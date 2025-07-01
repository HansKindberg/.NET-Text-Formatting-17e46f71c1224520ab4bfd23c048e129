//using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
//using HansKindberg.Text.Formatting.Extensions;
//using HansKindberg.Text.Formatting.Yaml.Core.Events.Extensions;
//using HansKindberg.Text.Formatting.Yaml.Core.Tokens.Extensions;
//using HansKindberg.Text.Formatting.Yaml.Models;
//using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
//using YamlDotNet.Core;
//using YamlDotNet.Core.Events;
//using IYamlDotNetParser = YamlDotNet.Core.IParser;
//using Scalar = YamlDotNet.Core.Events.Scalar;
//using TagDirective = YamlDotNet.Core.Tokens.TagDirective;
//using VersionDirective = YamlDotNet.Core.Tokens.VersionDirective;

//namespace HansKindberg.Text.Formatting.Yaml
//{
//	/// <inheritdoc />
//	public class YamlParser : IParser<IYamlNode>
//	{
//		#region Properties

//		protected internal virtual int InformationalYamlMaximumLength => 100;
//		protected internal virtual string InternalLeadingDocumentComment { get; } = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
//		protected internal virtual string InternalTrailingDocumentComment { get; } = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";

//		#endregion

//		#region Methods

//		protected internal virtual void ConsumeParsingEvents(IList<ParsingEvent> parsingEvents, IList<ParsingEvent> parsingEventsOnSameLine, params ParsingEvent[] parsingEventsToConsume)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			if(parsingEventsOnSameLine == null)
//				throw new ArgumentNullException(nameof(parsingEventsOnSameLine));

//			foreach(var parsingEventToConsume in parsingEventsToConsume)
//			{
//				parsingEvents.Remove(parsingEventToConsume);
//				parsingEventsOnSameLine.Remove(parsingEventToConsume);
//			}
//		}

//		protected internal virtual async Task<IYamlNode> CreateAnchorAliasValueNode(AnchorAlias anchorAlias)
//		{
//			await Task.CompletedTask;

//			return new YamlAnchorAliasValueNode(anchorAlias);
//		}

//		protected internal virtual async Task<IYamlDocumentNode> CreateDocumentNode(DocumentStart start, DocumentEnd end)
//		{
//			await Task.CompletedTask;

//			return new YamlDocumentNode(start, end);
//		}

//		protected internal virtual async Task<IYamlNode> CreateNode(IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			var comments = new List<Comment>();
//			var root = await this.CreateNodeForStream(parsingEvents);
//			var parent = root;

//			while(parsingEvents.Count > 0)
//			{
//				var parsingEvent = parsingEvents[0];
//				parsingEvents.RemoveAt(0);

//				switch(parsingEvent)
//				{
//					case AnchorAlias anchorAlias:
//					{
//						if(!parent.Sequence)
//							throw new InvalidOperationException("The parent for an anchor-alias must be a sequence.");

//						var node = await this.CreateNodeForAnchorAlias(anchorAlias, parsingEvents);
//						await this.TransferLeadingComments(comments, node);
//						await parent.Add(node);
//						break;
//					}
//					case Comment comment:
//					{
//						if(comment.IsInline)
//							throw new InvalidOperationException("Inline comment should have been handled earlier.");

//						comments.Add(comment);

//						break;
//					}
//					case DocumentEnd:
//					{

//						///////////////////////////////////////////////////////////await this.TransferTrailingComments(comments, root.Children.Last());

//						parent = root;
//						break;
//					}
//					case DocumentStart documentStart:
//					{
//						var node = await this.CreateNodeForDocumentStart(documentStart, parsingEvents);
//						await this.TransferLeadingComments(comments, node);
//						await parent.Add(node);
//						parent = node;
//						break;
//					}
//					case MappingEnd:
//					{
//						parent = parent.Parent ?? root;
//						break;
//					}
//					case MappingStart:
//					{
//						parent = root.Descendants().Last();
//						break;
//					}
//					case Scalar scalar:
//					{
//						var node = await this.CreateNodeForScalar(scalar, parsingEvents);
//						await this.TransferLeadingComments(comments, node);
//						await parent.Add(node);
//						break;
//					}
//					case SequenceEnd:
//					{
//						break;
//					}
//					case SequenceStart:
//					{
//						parent.Sequence = true;
//						break;
//					}
//					default:
//					{
//						throw new InvalidOperationException($"Invalid parsing-event: {parsingEvent.GetType()}");
//					}
//				}
//			}

//			await this.TransferTrailingComments(comments, root.Children.LastOrDefault() ?? root);

//			return root;
//		}

//		protected internal virtual async Task<IYamlNode> CreateNodeForAnchorAlias(AnchorAlias anchorAlias, IList<ParsingEvent> parsingEvents)
//		{
//			if(anchorAlias == null)
//				throw new ArgumentNullException(nameof(anchorAlias));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			var node = await this.CreateAnchorAliasValueNode(anchorAlias);

//			var parsingEventsOnSameLine = await this.GetParsingEventsOnSameLine(anchorAlias, parsingEvents);

//			if(this.TryConsumeComment(parsingEvents, parsingEventsOnSameLine, out var comment))
//				node.Comment = comment;

//			if(parsingEventsOnSameLine.Count > 0)
//				this.ThrowInvalidParsingEventsOnSameLineException(parsingEventsOnSameLine);

//			return node;
//		}

//		protected internal virtual async Task<IYamlDocumentNode> CreateNodeForDocumentStart(DocumentStart documentStart, IList<ParsingEvent> parsingEvents)
//		{
//			if(documentStart == null)
//				throw new ArgumentNullException(nameof(documentStart));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			var documentEnd = parsingEvents.OfType<DocumentEnd>().FirstOrDefault() ?? throw new InvalidOperationException("The document-end parsing-event is missing.");

//			var node = await this.CreateDocumentNode(documentStart, documentEnd);

//			await this.PopulateDocumentDirectives(documentStart, node);
//			await this.PopulateDirectiveComments(node.Directives, parsingEvents);
//			await this.PopulateDocumentComments(documentStart, documentEnd, node, parsingEvents);

//			return node;
//		}

//		protected internal virtual async Task<IYamlNode> CreateNodeForScalar(Scalar scalar, IList<ParsingEvent> parsingEvents)
//		{
//			if(scalar == null)
//				throw new ArgumentNullException(nameof(scalar));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			var parsingEventsOnSameLine = await this.GetParsingEventsOnSameLine(scalar, parsingEvents);

//			this.TryConsumeComment(parsingEvents, parsingEventsOnSameLine, out var comment);

//			if(!scalar.IsKey)
//				return await this.CreateScalarValueNode(scalar, comment);

//			// When it is like this for example: "key: {value-1: null, value-2: null}" or "key: {value-1, value-2}"
//			var isInlineMappingValue = parsingEventsOnSameLine.FirstOrDefault() is MappingStart;
//			// When it is like this for example: "key: [value-1, value-2]"
//			var isInlineSequenceValue = parsingEventsOnSameLine.FirstOrDefault() is SequenceStart;

//			if(isInlineMappingValue || isInlineSequenceValue || !parsingEventsOnSameLine.Any())
//				return await this.CreateScalarKeyNode(scalar, comment);

//			if(this.TryConsumeAnchorAlias(parsingEvents, parsingEventsOnSameLine, out var anchorAliasValue))
//				return await this.CreateScalarKeyAnchorAliasValuePairNode(scalar, anchorAliasValue!, comment);

//			if(this.TryConsumeScalarValue(parsingEvents, parsingEventsOnSameLine, out var scalarValue))
//				return await this.CreateScalarKeyScalarValuePairNode(scalar, scalarValue!, comment);

//			return await this.CreateScalarKeyNode(scalar, comment);
//		}

//		protected internal virtual async Task<IYamlNode> CreateNodeForStream(IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			if(parsingEvents.Count < 2)
//				throw new ArgumentException("The parsing-events must contain at least a stream-start and a stream-end.", nameof(parsingEvents));

//			if(parsingEvents[0] is not StreamStart streamStart)
//				throw new ArgumentException("The first parsing-event must be a stream-start.", nameof(parsingEvents));

//			if(parsingEvents.Last() is not StreamEnd streamEnd)
//				throw new ArgumentException("The last parsing-event must be a stream-end.", nameof(parsingEvents));

//			parsingEvents.Remove(streamStart);
//			parsingEvents.Remove(streamEnd);

//			return await this.CreateStreamNode(streamStart, streamEnd);
//		}

//		protected internal virtual async Task<IList<ParsingEvent>> CreateParsingEvents(string value, IList<int>? parsingEventIndexesToRemove = null)
//		{
//			if(value == null)
//				throw new ArgumentNullException(nameof(value));

//			var parsingEvents = new List<ParsingEvent>();

//			using(var stringReader = new StringReader(value))
//			{
//				var yamlDotNetParser = await this.CreateYamlDotNetParser(stringReader);

//				try
//				{
//					while(yamlDotNetParser.MoveNext())
//					{
//						parsingEvents.Add(yamlDotNetParser.Current!);
//					}
//				}
//				catch(InvalidOperationException invalidOperationException)
//				{
//					// Special case 1
//					// If the value starts with:
//					//   ...
//					//   ---
//					// or
//					//   # Comment
//					//   ...
//					//   ---
//					if(invalidOperationException.Message == "The scanner should contain no more tokens.")
//						return await this.CreateParsingEvents($"---{Environment.NewLine}...{Environment.NewLine}{value}", [1, 2, 3]);

//					throw;
//				}
//			}

//			if(parsingEventIndexesToRemove != null && parsingEventIndexesToRemove.Any())
//			{
//				foreach(var parsingEventIndexToRemove in parsingEventIndexesToRemove.OrderByDescending(item => item))
//				{
//					parsingEvents.RemoveAt(parsingEventIndexToRemove);
//				}
//			}

//			// Special case 2
//			if(await this.IsOnlyCommentsYaml(parsingEvents))
//				return await this.CreateParsingEvents($"--- # {this.InternalLeadingDocumentComment}{Environment.NewLine}{value}", parsingEventIndexesToRemove);

//			// Special case 3
//			if(parsingEvents.Last() is not StreamEnd)
//				return await this.CreateParsingEvents($"{value}{Environment.NewLine}--- # {this.InternalTrailingDocumentComment}", parsingEventIndexesToRemove);

//			// Resolve parsing-events. Remove clutter, change position etc.
//			await this.ResolveParsingEvents(parsingEvents);

//			//////////////////////// Put comments on the right place.
//			//////////////////////await this.ResolveComments(parsingEvents);

//			//////////////////////await this.SettleHomelessParsingEvents(parsingEvents);

//			return parsingEvents;
//		}

//		protected internal virtual async Task<IYamlNode> CreateScalarKeyAnchorAliasValuePairNode(Scalar key, AnchorAlias value, Comment? comment = null)
//		{
//			await Task.CompletedTask;

//			return new YamlScalarKeyAnchorAliasValuePairNode(key, value)
//			{
//				Comment = comment
//			};
//		}

//		protected internal virtual async Task<IYamlNode> CreateScalarKeyNode(Scalar key, Comment? comment = null)
//		{
//			await Task.CompletedTask;

//			return new YamlScalarKeyNode(key)
//			{
//				Comment = comment
//			};
//		}

//		protected internal virtual async Task<IYamlNode> CreateScalarKeyScalarValuePairNode(Scalar key, Scalar value, Comment? comment = null)
//		{
//			await Task.CompletedTask;

//			return new YamlScalarKeyScalarValuePairNode(key, value)
//			{
//				Comment = comment
//			};
//		}

//		protected internal virtual async Task<IYamlNode> CreateScalarValueNode(Scalar value, Comment? comment = null)
//		{
//			await Task.CompletedTask;

//			return new YamlScalarValueNode(value)
//			{
//				Comment = comment
//			};
//		}

//		protected internal virtual async Task<IScanner> CreateScanner(TextReader textReader)
//		{
//			await Task.CompletedTask;

//			return new Scanner(textReader, false);
//		}

//		protected internal virtual async Task<IYamlNode> CreateStreamNode(StreamStart start, StreamEnd end)
//		{
//			await Task.CompletedTask;

//			return new YamlStreamNode(start, end);
//		}

//		protected internal virtual async Task<IYamlDirective> CreateTagDirective(TagDirective tag)
//		{
//			await Task.CompletedTask;

//			return new YamlTagDirective(tag);
//		}

//		protected internal virtual async Task<IYamlDirective> CreateVersionDirective(VersionDirective version)
//		{
//			await Task.CompletedTask;

//			return new YamlVersionDirective(version);
//		}

//		protected internal virtual async Task<IYamlDotNetParser> CreateYamlDotNetParser(TextReader textReader)
//		{
//			return new Parser(await this.CreateScanner(textReader));
//		}

//		////////////protected internal virtual async Task<DocumentStart?> GetClosestFolowingDocumentStart(ParsingEvent parsingEvent, IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvent == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvent));

//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	await Task.CompletedTask;

//		////////////	return parsingEvents.OfType<DocumentStart>().FirstOrDefault(documentStart => documentStart.Start.Line > parsingEvent.End.Line);
//		////////////}

//		////////////protected internal virtual async Task<IList<KeyValuePair<DocumentStart, DocumentEnd>>> GetDocumentSpans(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	await Task.CompletedTask;

//		////////////	DocumentStart? currentDocumentStart = null;
//		////////////	var documentSpans = new List<KeyValuePair<DocumentStart, DocumentEnd>>();

//		////////////	foreach(var parsingEvent in parsingEvents)
//		////////////	{
//		////////////		switch(parsingEvent)
//		////////////		{
//		////////////			case DocumentStart documentStart:
//		////////////				currentDocumentStart = documentStart;
//		////////////				continue;
//		////////////			case DocumentEnd documentEnd:
//		////////////				documentSpans.Add(new KeyValuePair<DocumentStart, DocumentEnd>(currentDocumentStart!, documentEnd));
//		////////////				currentDocumentStart = null;
//		////////////				break;
//		////////////			default:
//		////////////				break;
//		////////////		}
//		////////////	}

//		////////////	return documentSpans;
//		////////////}

//		////////////protected internal virtual async Task<IList<SortedSet<long>>> GetHomelessLineGroups(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	var homelessLineGroups = new List<SortedSet<long>>();
//		////////////	var homelessLines = await this.GetHomelessLines(parsingEvents);

//		////////////	if(homelessLines.Count < 1)
//		////////////		return homelessLineGroups;

//		////////////	long? previousHomelessLine = null;

//		////////////	foreach(var homelessLine in homelessLines)
//		////////////	{
//		////////////		var homelessLineGroup = homelessLineGroups.LastOrDefault();

//		////////////		if(homelessLineGroup == null || homelessLine > previousHomelessLine + 1)
//		////////////		{
//		////////////			homelessLineGroup = [];
//		////////////			homelessLineGroups.Add(homelessLineGroup);
//		////////////		}

//		////////////		homelessLineGroup.Add(homelessLine);

//		////////////		previousHomelessLine = homelessLine;
//		////////////	}

//		////////////	return homelessLineGroups;
//		////////////}

//		////////////protected internal virtual async Task<SortedSet<long>> GetHomelessLines(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	var homelessLines = new SortedSet<long>();
//		////////////	var settledLines = await this.GetSettledLines(parsingEvents);

//		////////////	for(long i = 1; i <= parsingEvents.Max(parsingEvent => parsingEvent.End.Line); i++)
//		////////////	{
//		////////////		if(settledLines.Contains(i))
//		////////////			continue;

//		////////////		homelessLines.Add(i);
//		////////////	}

//		////////////	return homelessLines;
//		////////////}

//		////////////protected internal virtual async Task<IList<IList<ParsingEvent>>> GetHomelessParsingEventGroups(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	var homelessLineGroups = await this.GetHomelessLineGroups(parsingEvents);
//		////////////	var homelessParsingEventGroups = new List<IList<ParsingEvent>>();
//		////////////	var parsingEventTypesToExclude = new HashSet<Type> { typeof(DocumentEnd), typeof(DocumentStart), typeof(StreamEnd), typeof(StreamStart) };

//		////////////	foreach(var homelessLineGroup in homelessLineGroups)
//		////////////	{
//		////////////		var homelessParsingEventGroup = new List<ParsingEvent>();

//		////////////		foreach(var homelessLine in homelessLineGroup)
//		////////////		{
//		////////////			var homelessParsingEvents = parsingEvents.Where(parsingEvent => parsingEvent.Start.Line == homelessLine || parsingEvent.End.Line == homelessLine).ToList();

//		////////////			homelessParsingEvents = [.. homelessParsingEvents.Where(parsingEvent => !parsingEventTypesToExclude.Contains(parsingEvent.GetType()))];

//		////////////			if(homelessParsingEvents.Count < 1)
//		////////////				continue;

//		////////////			homelessParsingEventGroup.AddRange(homelessParsingEvents);
//		////////////		}

//		////////////		if(homelessParsingEventGroup.Count > 0)
//		////////////			homelessParsingEventGroups.Add(homelessParsingEventGroup);
//		////////////	}

//		////////////	return homelessParsingEventGroups;
//		////////////}

//		protected internal virtual string GetInvalidParsingEventsOnSameLineExceptionMessage(IList<ParsingEvent> parsingEvents)
//		{
//			return $"Invalid parsing-events on same line: {string.Join(", ", parsingEvents.Select(parsingEvent => parsingEvent.GetType()))}";
//		}

//		protected internal virtual async Task<IList<ParsingEvent>> GetParsingEventsOnSameLine(ParsingEvent parsingEvent, IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvent == null)
//				throw new ArgumentNullException(nameof(parsingEvent));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			await Task.CompletedTask;

//			return [.. parsingEvents.Where(item => parsingEvent.Start.Line == item.Start.Line)];
//		}

//		////////////protected internal virtual async Task<SortedSet<long>> GetSettledLines(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	var documentSpans = await this.GetDocumentSpans(parsingEvents);
//		////////////	var settledLines = new SortedSet<long>();

//		////////////	foreach(var documentSpan in documentSpans)
//		////////////	{
//		////////////		for(var i = documentSpan.Key.Start.Line; i <= documentSpan.Value.End.Line; i++)
//		////////////		{
//		////////////			settledLines.Add(i);
//		////////////		}
//		////////////	}

//		////////////	return settledLines;
//		////////////}

//		protected internal virtual string GetStringRepresentation(string? value)
//		{
//			if(value != null && value.Length > this.InformationalYamlMaximumLength)
//				value = $"{value.Substring(0, this.InformationalYamlMaximumLength)}...";

//			return value.ToStringRepresentation();
//		}

//		/// <summary>
//		/// If the text contains only comments we will only get two parsing-events, a stream-start and a comment (the first one).
//		/// </summary>
//		protected internal virtual async Task<bool> IsOnlyCommentsYaml(IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			await Task.CompletedTask;

//			if(parsingEvents.Count != 2)
//				return false;

//			if(parsingEvents[0] is not StreamStart)
//				return false;

//			if(parsingEvents[1] is not Comment)
//				return false;

//			return true;
//		}

//		public virtual async Task<IYamlNode> Parse(string value)
//		{
//			if(value == null)
//				throw new ArgumentNullException(nameof(value));

//			try
//			{
//				var parsingEvents = await this.CreateParsingEvents(value);

//				var node = await this.CreateNode(parsingEvents);

//				return node;
//			}
//			catch(Exception exception)
//			{
//				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-node.", exception);
//			}
//		}

//		protected internal virtual async Task PopulateDirectiveComments(IList<IYamlDirective> directives, IList<ParsingEvent> parsingEvents)
//		{
//			if(directives == null)
//				throw new ArgumentNullException(nameof(directives));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			await Task.CompletedTask;

//			foreach(var directive in directives)
//			{
//				var comment = parsingEvents.OfType<Comment>().FirstOrDefault(item => item.Start.Line == directive.Start.Line);

//				if(comment == null)
//					continue;

//				if(comment.IsInline)
//					continue;

//				directive.Comment = comment;

//				parsingEvents.Remove(comment);
//			}
//		}

//		protected internal virtual async Task PopulateDocumentComments(DocumentStart documentStart, DocumentEnd documentEnd, IYamlDocumentNode node, IList<ParsingEvent> parsingEvents)
//		{
//			if(documentStart == null)
//				throw new ArgumentNullException(nameof(documentStart));

//			if(documentEnd == null)
//				throw new ArgumentNullException(nameof(documentEnd));

//			if(node == null)
//				throw new ArgumentNullException(nameof(node));

//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			await Task.CompletedTask;

//			if(!documentStart.IsImplicit)
//			{
//				var commentsBeforeDocumentStart = parsingEvents.Where(parsingEvent => parsingEvent.Start.Line < documentStart.Start.Line).OfType<Comment>().ToList();
//				node.LeadingComments.AddRange(commentsBeforeDocumentStart);

//				foreach(var commentBeforeDocumentStart in commentsBeforeDocumentStart)
//				{
//					parsingEvents.Remove(commentBeforeDocumentStart);
//				}

//				var documentStartComment = parsingEvents.OfType<Comment>().FirstOrDefault(comment => comment.Start.Line == documentStart.End.Line);
//				if(documentStartComment != null)
//				{
//					node.Comment = documentStartComment;
//					parsingEvents.Remove(documentStartComment);
//				}
//			}

//			if(documentEnd.IsImplicit)
//				return;

//			var documentEndComment = parsingEvents.OfType<Comment>().FirstOrDefault(comment => comment.Start.Line == documentEnd.Start.Line);
//			if(documentEndComment != null)
//			{
//				node.EndComment = documentEndComment;
//				parsingEvents.Remove(documentEndComment);
//			}
//		}

//		protected internal virtual async Task PopulateDocumentDirectives(DocumentStart documentStart, IYamlDocumentNode node)
//		{
//			if(documentStart == null)
//				throw new ArgumentNullException(nameof(documentStart));

//			if(node == null)
//				throw new ArgumentNullException(nameof(node));

//			if(documentStart.Version != null)
//				node.Directives.Add(await this.CreateVersionDirective(documentStart.Version));

//			if(documentStart.Tags != null)
//			{
//				foreach(var tag in documentStart.Tags)
//				{
//					if(tag.IsDefault())
//						continue;

//					node.Directives.Add(await this.CreateTagDirective(tag));
//				}
//			}
//		}

//		protected internal virtual async Task ResolveComments(IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			await Task.CompletedTask;

//			var parsingEventsOrderedByStartLine = parsingEvents.OrderBy(parsingEvent => parsingEvent.Start.Line).ToList();

//			for(var i = parsingEventsOrderedByStartLine.Count - 1; i >= 0; i--)
//			{
//				var orderedParsingEvent = parsingEventsOrderedByStartLine[i];

//				if(parsingEvents[i] == orderedParsingEvent)
//					continue;

//				parsingEvents.Remove(orderedParsingEvent);
//				parsingEvents.Insert(i, orderedParsingEvent);
//			}

//			var explicitDocumentEnds = parsingEvents.OfType<DocumentEnd>().Where(documentEnd => !documentEnd.IsImplicit).ToList();

//			foreach(var documentEnd in explicitDocumentEnds)
//			{
//				var documentEndComment = parsingEvents.OfType<Comment>().FirstOrDefault(comment => comment.Start.Line == documentEnd.Start.Line);

//				if(documentEndComment == null)
//					continue;

//				parsingEvents.Remove(documentEndComment);

//				var index = parsingEvents.IndexOf(documentEnd);

//				parsingEvents.Insert(index, new Comment(documentEndComment.Value, true, documentEndComment.Start, documentEndComment.End));
//			}
//		}

//		protected internal virtual async Task ResolveParsingEvents(IList<ParsingEvent> parsingEvents)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			var internalLeadingDocumentCommentExists = false;
//			var internalTrailingDocumentCommentExists = false;

//			for(var i = parsingEvents.Count - 1; i >= 0; i--)
//			{
//				if(parsingEvents[i] is Comment leadingComment && string.Equals(leadingComment.Value, this.InternalLeadingDocumentComment, StringComparison.OrdinalIgnoreCase))
//				{
//					parsingEvents.RemoveAt(i);
//					internalLeadingDocumentCommentExists = true;
//					continue;
//				}

//				if(parsingEvents[i] is Comment trailingComment && string.Equals(trailingComment.Value, this.InternalTrailingDocumentComment, StringComparison.OrdinalIgnoreCase))
//				{
//					parsingEvents.RemoveAt(i);
//					internalTrailingDocumentCommentExists = true;
//					continue;
//				}

//				if(parsingEvents[i] is Scalar scalar && scalar.IsEmpty())
//				{
//					parsingEvents.RemoveAt(i);
//				}
//			}

//			if(internalLeadingDocumentCommentExists)
//				parsingEvents[1] = new DocumentStart(); // Make the first document-start, that was added internally, to be implicit.

//			if(internalTrailingDocumentCommentExists)
//			{
//				parsingEvents.Remove(parsingEvents.OfType<DocumentEnd>().Last()); // Remove the last document-end that was added internally.
//				parsingEvents.Remove(parsingEvents.OfType<DocumentStart>().Last()); // Remove the last document-start that was added internally.
//			}

//			await this.ResolveComments(parsingEvents);
//		}

//		/////////////// <summary>
//		/////////////// If we have parsing-events that is not "inside" a document we put them inside a document. This probably only concerns comments.
//		/////////////// </summary>
//		////////////protected internal virtual async Task SettleHomelessParsingEvents(IList<ParsingEvent> parsingEvents)
//		////////////{
//		////////////	if(parsingEvents == null)
//		////////////		throw new ArgumentNullException(nameof(parsingEvents));

//		////////////	var homelessParsingEventGroups = await this.GetHomelessParsingEventGroups(parsingEvents);

//		////////////	if(homelessParsingEventGroups.Count < 1)
//		////////////		return;

//		////////////	foreach(var homelessParsingEventGroup in homelessParsingEventGroups)
//		////////////	{
//		////////////		foreach(var parsingEvent in homelessParsingEventGroup)
//		////////////		{
//		////////////			parsingEvents.Remove(parsingEvent);
//		////////////		}

//		////////////		var firstHomelessParsingEvent = homelessParsingEventGroup.First();

//		////////////		var closestFollowingDocumentStart = await this.GetClosestFolowingDocumentStart(firstHomelessParsingEvent, parsingEvents);

//		////////////		var index = closestFollowingDocumentStart != null ? parsingEvents.IndexOf(closestFollowingDocumentStart) : parsingEvents.Count - 1;

//		////////////		var reversedHomelessParsingEvents = homelessParsingEventGroup.Reverse().ToList();

//		////////////		parsingEvents.Insert(index, new DocumentEnd(true));

//		////////////		foreach(var parsingEvent in reversedHomelessParsingEvents)
//		////////////		{
//		////////////			parsingEvents.Insert(index, parsingEvent);
//		////////////		}

//		////////////		parsingEvents.Insert(index, new DocumentStart());
//		////////////	}
//		////////////}

//		protected internal virtual void ThrowInvalidParsingEventsOnSameLineException(IList<ParsingEvent> parsingEvents)
//		{
//			throw new InvalidOperationException(this.GetInvalidParsingEventsOnSameLineExceptionMessage(parsingEvents));
//		}

//		protected internal virtual async Task TransferComments(IList<Comment> comments, IList<Comment> destination)
//		{
//			if(comments == null)
//				throw new ArgumentNullException(nameof(comments));

//			if(destination == null)
//				throw new ArgumentNullException(nameof(destination));

//			await Task.CompletedTask;

//			if(!comments.Any())
//				return;

//			destination.AddRange(comments);
//			comments.Clear();
//		}

//		protected internal virtual async Task TransferLeadingComments(IList<Comment> comments, IYamlNode node)
//		{
//			if(comments == null)
//				throw new ArgumentNullException(nameof(comments));

//			if(node == null)
//				throw new ArgumentNullException(nameof(node));

//			await this.TransferComments(comments, node.LeadingComments);
//		}

//		protected internal virtual async Task TransferTrailingComments(IList<Comment> comments, IYamlNode node)
//		{
//			if(comments == null)
//				throw new ArgumentNullException(nameof(comments));

//			if(node == null)
//				throw new ArgumentNullException(nameof(node));

//			await this.TransferComments(comments, node.TrailingComments);
//		}

//		protected internal virtual bool TryConsumeAnchorAlias(IList<ParsingEvent> parsingEvents, IList<ParsingEvent> parsingEventsOnSameLine, out AnchorAlias? anchorAlias)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			if(parsingEventsOnSameLine == null)
//				throw new ArgumentNullException(nameof(parsingEventsOnSameLine));

//			anchorAlias = parsingEventsOnSameLine.FirstOrDefault() as AnchorAlias;

//			if(anchorAlias == null)
//				return false;

//			this.ConsumeParsingEvents(parsingEvents, parsingEventsOnSameLine, anchorAlias);

//			return true;
//		}

//		/// <summary>
//		/// Try to consume the last parsing-event on the same line as an inline comment.
//		/// </summary>
//		protected internal virtual bool TryConsumeComment(IList<ParsingEvent> parsingEvents, IList<ParsingEvent> parsingEventsOnSameLine, out Comment? comment)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			if(parsingEventsOnSameLine == null)
//				throw new ArgumentNullException(nameof(parsingEventsOnSameLine));

//			comment = parsingEventsOnSameLine.LastOrDefault() as Comment;

//			if(comment is not { IsInline: true })
//				return false;

//			this.ConsumeParsingEvents(parsingEvents, parsingEventsOnSameLine, comment);

//			return true;
//		}

//		protected internal virtual bool TryConsumeScalarValue(IList<ParsingEvent> parsingEvents, IList<ParsingEvent> parsingEventsOnSameLine, out Scalar? scalar)
//		{
//			if(parsingEvents == null)
//				throw new ArgumentNullException(nameof(parsingEvents));

//			if(parsingEventsOnSameLine == null)
//				throw new ArgumentNullException(nameof(parsingEventsOnSameLine));

//			scalar = parsingEventsOnSameLine.FirstOrDefault() as Scalar;

//			if(scalar is { IsKey: true })
//				return false;

//			this.ConsumeParsingEvents(parsingEvents, parsingEventsOnSameLine, scalar!);

//			return true;
//		}

//		#endregion
//	}
//}

