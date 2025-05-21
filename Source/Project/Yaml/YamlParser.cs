using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Extensions;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using IYamlDotNetParser = YamlDotNet.Core.IParser;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class YamlParser : IParser<IList<IYamlNode>>
	{
		#region Properties

		protected internal virtual int InformationalYamlMaximumLength => 100;
		protected internal virtual string LeadingDocumentComment { get; } = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";

		#endregion

		#region Methods

		protected internal virtual async Task<IList<ParsingEvent>> ConsumeParsingEventsOnSameLine(ParsingEvent parsingEvent, IList<ParsingEvent> parsingEvents)
		{
			if(parsingEvent == null)
				throw new ArgumentNullException(nameof(parsingEvent));

			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			await Task.CompletedTask;

			var parsingEventsOnSameLine = new List<ParsingEvent>();

			while(parsingEvents.Count > 0 && parsingEvent.Start.Line == parsingEvents[0].Start.Line)
			{
				parsingEventsOnSameLine.Add(parsingEvents[0]);
				parsingEvents.RemoveAt(0);
			}

			return parsingEventsOnSameLine;
		}

		protected internal virtual async Task<IList<IYamlNode>> CreateDocumentNodes(IDictionary<IYamlNode, IList<ParsingEvent>> documents)
		{
			var nodes = new List<IYamlNode>();

			foreach(var document in documents)
			{
				await this.PopulateDocumentNode(document.Key, document.Value);

				if(!document.Key.Children.Any() && document.Key.Comment == null && !document.Key.LeadingComments.Any() && !document.Key.TrailingComments.Any())
					continue;

				nodes.Add(document.Key);
			}

			return nodes;
		}

		protected internal virtual async Task<IYamlNode> CreateNodeForAnchorAlias(AnchorAlias anchorAlias, IList<ParsingEvent> parsingEvents)
		{
			if(anchorAlias == null)
				throw new ArgumentNullException(nameof(anchorAlias));

			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			var parsingEventsOnSameLine = await this.ConsumeParsingEventsOnSameLine(anchorAlias, parsingEvents);

			var node = new YamlAnchorAliasValueNode(anchorAlias);

			if(this.TryConsumeComment(parsingEventsOnSameLine, out var comment))
				node.Comment = comment!.Value.Trim();
			else if(parsingEventsOnSameLine.Count > 0)
				this.ThrowInvalidParsingEventsOnSameLineException(parsingEventsOnSameLine);

			return node;
		}

		protected internal virtual async Task<IYamlNode> CreateNodeForScalar(Scalar scalar, IList<ParsingEvent> parsingEvents, bool sequence)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			if(!scalar.IsKey && !sequence)
				throw new InvalidOperationException("The scalar must be a key because the parent is not a sequence.");

			var parsingEventsOnSameLine = await this.ConsumeParsingEventsOnSameLine(scalar, parsingEvents);

			if(!scalar.IsKey)
			{
				if(this.TryConsumeComment(parsingEventsOnSameLine, out var valueComment))
				{
					return new YamlScalarValueNode(scalar)
					{
						Comment = valueComment!.Value.Trim()
					};
				}

				if(parsingEventsOnSameLine.Any())
					throw new InvalidOperationException(this.GetInvalidParsingEventsOnSameLineExceptionMessage(parsingEventsOnSameLine));

				return new YamlScalarValueNode(scalar);
			}

			if(this.TryConsumeAnchorAlias(parsingEventsOnSameLine, out var anchorAliasValue))
				return new YamlScalarKeyAnchorAliasValuePairNode(scalar, anchorAliasValue!);

			if(this.TryConsumeAnchorAliasWithComment(parsingEventsOnSameLine, out anchorAliasValue, out var keyComment))
			{
				return new YamlScalarKeyAnchorAliasValuePairNode(scalar, anchorAliasValue!)
				{
					Comment = keyComment!.Value.Trim()
				};
			}

			if(this.TryConsumeComment(parsingEventsOnSameLine, out keyComment))
			{
				return new YamlScalarKeyNode(scalar)
				{
					Comment = keyComment!.Value.Trim()
				};
			}

			if(this.TryConsumeScalarValue(parsingEventsOnSameLine, out var scalarValue))
				return new YamlScalarKeyScalarValuePairNode(scalar, scalarValue!);

			if(this.TryConsumeScalarValueWithComment(parsingEventsOnSameLine, out scalarValue, out keyComment))
			{
				return new YamlScalarKeyScalarValuePairNode(scalar, scalarValue!)
				{
					Comment = keyComment!.Value.Trim()
				};
			}

			if(parsingEventsOnSameLine.Any())
				throw new InvalidOperationException(this.GetInvalidParsingEventsOnSameLineExceptionMessage(parsingEventsOnSameLine));

			return new YamlScalarKeyNode(scalar);
		}

		protected internal virtual async Task<IList<ParsingEvent>> CreateParsingEvents(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			var parsingEvents = new List<ParsingEvent>();

			using(var stringReader = new StringReader(value))
			{
				var yamlDotNetParser = await this.CreateYamlDotNetParser(stringReader);

				while(yamlDotNetParser.MoveNext())
				{
					parsingEvents.Add(yamlDotNetParser.Current!);
				}
			}

			if(await this.IsOnlyCommentsYaml(parsingEvents))
				return await this.CreateParsingEvents($"--- # {this.LeadingDocumentComment}{Environment.NewLine}{value}");

			return parsingEvents;
		}

		protected internal virtual async Task<IScanner> CreateScanner(TextReader textReader)
		{
			await Task.CompletedTask;

			return new Scanner(textReader, false);
		}

		protected internal virtual async Task<IYamlDotNetParser> CreateYamlDotNetParser(TextReader textReader)
		{
			return new Parser(await this.CreateScanner(textReader));
		}

		protected internal virtual string GetInvalidParsingEventsOnSameLineExceptionMessage(IList<ParsingEvent> parsingEvents)
		{
			return $"Invalid parsing-events on same line: {string.Join(", ", parsingEvents.Select(parsingEvent => parsingEvent.GetType()))}";
		}

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value != null && value.Length > this.InformationalYamlMaximumLength)
				value = $"{value.Substring(0, this.InformationalYamlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		/// <summary>
		/// If the text contains only comments we will only get two parsing-events, a stream-start and a comment (the first one).
		/// </summary>
		protected internal virtual async Task<bool> IsOnlyCommentsYaml(IList<ParsingEvent> parsingEvents)
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			await Task.CompletedTask;

			if(parsingEvents.Count != 2)
				return false;

			if(parsingEvents[0] is not StreamStart)
				return false;

			if(parsingEvents[1] is not Comment)
				return false;

			return true;
		}

		/// <summary>
		/// Each yaml-document in the parsed text gives a node in the list.
		/// </summary>
		public virtual async Task<IList<IYamlNode>> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				var parsingEvents = await this.CreateParsingEvents(value);

				var documents = await this.SplitIntoDocuments(parsingEvents);

				await this.ResolveDocuments(documents);

				var nodes = await this.CreateDocumentNodes(documents);

				return nodes;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-node-list.", exception);
			}
		}

		protected internal virtual async Task PopulateDocumentNode(IYamlNode document, IList<ParsingEvent> parsingEvents)
		{
			var leadingCommentsBuffer = new List<string>();
			var parent = document;

			while(parsingEvents.Count > 0)
			{
				var parsingEvent = parsingEvents[0];
				parsingEvents.RemoveAt(0);

				switch(parsingEvent)
				{
					case AnchorAlias anchorAlias:
					{
						if(!parent.Sequence)
							throw new InvalidOperationException("The parent for an anchor-alias must be a sequence.");

						var node = await this.CreateNodeForAnchorAlias(anchorAlias, parsingEvents);
						await this.TransferLeadingCommentsIfNecesarry(leadingCommentsBuffer, node);
						await parent.Add(node);
						break;
					}
					case Comment comment:
					{
						if(comment.IsInline)
							throw new InvalidOperationException("Inline comment should have been handled earlier.");

						var value = comment.Value.Trim();

						if(parsingEvents.Any(item => item is Scalar)) // If there is any scalar parsing-event after the comment we add it as a leading comment, otherwise as a trailing comment.
						{
							leadingCommentsBuffer.Add(value);
						}
						else
						{
							var node = document.Descendants().LastOrDefault() ?? document;
							node.TrailingComments.Add(value);
						}

						break;
					}
					case MappingEnd:
					{
						parent = parent.Parent ?? document;
						break;
					}
					case MappingStart:
					{
						parent = document.Descendants().LastOrDefault() ?? document;
						break;
					}
					case Scalar scalar:
					{
						var node = await this.CreateNodeForScalar(scalar, parsingEvents, parent.Sequence);
						await this.TransferLeadingCommentsIfNecesarry(leadingCommentsBuffer, node);
						await parent.Add(node);
						break;
					}
					case SequenceEnd:
					{
						break;
					}
					case SequenceStart:
					{
						parent.Sequence = true;
						break;
					}
					default:
					{
						throw new InvalidOperationException($"Invalid parsing-event: {parsingEvent.GetType()}");
					}
				}
			}
		}

		protected internal virtual async Task ResolveDocuments(IDictionary<IYamlNode, IList<ParsingEvent>> documents)
		{
			if(documents == null)
				throw new ArgumentNullException(nameof(documents));

			await Task.CompletedTask;

			foreach(var document in documents)
			{
				for(var i = document.Value.Count - 1; i >= 0; i--)
				{
					// If the text we want to parse contains only comments or only empty lines we may get an "empty" scalar parsing-event that we need to remove. We find this scalar parsing-event with the following criteria.
					if(document.Value[i] is Scalar { IsPlainImplicit: true, Style: ScalarStyle.Plain, Value: "" } scalar && scalar.End.Column == scalar.Start.Column && scalar.End.Index == scalar.Start.Index && scalar.End.Line == scalar.Start.Line)
						document.Value.RemoveAt(i);
				}
			}
		}

		protected internal virtual async Task ResolveLeadingDocumentStart(IList<ParsingEvent> parsingEvents)
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			await Task.CompletedTask;

			var documentStart = parsingEvents.OfType<DocumentStart>().FirstOrDefault();

			if(documentStart == null)
				return;

			var leadingDocumentStartIndex = parsingEvents.IndexOf(documentStart);
			var leadingDocumentCommentIndex = leadingDocumentStartIndex + 1;

			if(leadingDocumentCommentIndex < 1 || leadingDocumentCommentIndex >= parsingEvents.Count)
				return;

			if(parsingEvents[leadingDocumentCommentIndex] is Comment { IsInline: true } comment && string.Equals(comment.Value, this.LeadingDocumentComment, StringComparison.OrdinalIgnoreCase))
			{
				parsingEvents.RemoveAt(leadingDocumentCommentIndex);
				parsingEvents[leadingDocumentStartIndex] = new DocumentStart(null, [], true, documentStart.Start, documentStart.End);
			}
		}

		protected internal virtual async Task<IDictionary<IYamlNode, IList<ParsingEvent>>> SplitIntoDocuments(IList<ParsingEvent> parsingEvents)
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			var documents = new Dictionary<IYamlNode, IList<ParsingEvent>>();

			if(parsingEvents.Any())
			{
				await this.ValidateAndTrimStreamEvents(parsingEvents);

				if(parsingEvents.Any())
				{
					await this.ResolveLeadingDocumentStart(parsingEvents);

					var documentEndIndexes = new List<int>();
					var documentStartIndexes = new List<int>();

					for(var i = 0; i < parsingEvents.Count; i++)
					{
						if(parsingEvents[i] is DocumentEnd)
						{
							documentEndIndexes.Add(i);
							continue;
						}

						if(parsingEvents[i] is DocumentStart)
							documentStartIndexes.Add(i);
					}

					var valid = documentEndIndexes.Count > 0 && documentEndIndexes.Count == documentStartIndexes.Count;

					if(valid)
					{
						if(documentEndIndexes.Last() != parsingEvents.Count - 1 || documentStartIndexes[0] != 0)
							valid = false;

						if(documentStartIndexes.Skip(1).Any(index => !documentEndIndexes.Contains(index - 1)))
							valid = false;
					}

					if(!valid)
						throw new ArgumentException("The parsing-events contains an invalid document structure.", nameof(parsingEvents));

					for(var i = 0; i < documentStartIndexes.Count; i++)
					{
						var documentStartIndex = documentStartIndexes[i];
						var documentEndIndex = documentEndIndexes[i];

						var document = new YamlDocumentNode((DocumentStart)parsingEvents[documentStartIndex], (DocumentEnd)parsingEvents[documentEndIndex]);
						documents.Add(document, [.. parsingEvents.Skip(documentStartIndex + 1).Take(documentEndIndex - documentStartIndex - 1)]);
					}
				}
			}

			return documents;
		}

		protected internal virtual void ThrowInvalidParsingEventsOnSameLineException(IList<ParsingEvent> parsingEvents)
		{
			throw new InvalidOperationException(this.GetInvalidParsingEventsOnSameLineExceptionMessage(parsingEvents));
		}

		protected internal virtual async Task TransferLeadingCommentsIfNecesarry(IList<string> leadingCommentsBuffer, IYamlNode node)
		{
			if(leadingCommentsBuffer == null)
				throw new ArgumentNullException(nameof(leadingCommentsBuffer));

			if(node == null)
				throw new ArgumentNullException(nameof(node));

			await Task.CompletedTask;

			if(!leadingCommentsBuffer.Any())
				return;

			node.LeadingComments.AddRange(leadingCommentsBuffer);
			leadingCommentsBuffer.Clear();
		}

		protected internal virtual bool TryConsume<T>(IList<ParsingEvent> parsingEvents, out T? parsingEvent) where T : ParsingEvent
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			parsingEvent = null;

			if(parsingEvents.Count != 1)
				return false;

			parsingEvent = parsingEvents[0] as T;

			return parsingEvent != null;
		}

		protected internal virtual bool TryConsume<T1, T2>(IList<ParsingEvent> parsingEvents, out T1? firstParsingEvent, out T2? secondParsingEvent) where T1 : ParsingEvent where T2 : ParsingEvent
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			firstParsingEvent = null;
			secondParsingEvent = null;

			if(parsingEvents.Count != 2)
				return false;

			firstParsingEvent = parsingEvents[0] as T1;
			secondParsingEvent = parsingEvents[1] as T2;

			return firstParsingEvent != null && secondParsingEvent != null;
		}

		protected internal virtual bool TryConsumeAnchorAlias(IList<ParsingEvent> parsingEvents, out AnchorAlias? anchorAlias)
		{
			return this.TryConsume(parsingEvents, out anchorAlias);
		}

		protected internal virtual bool TryConsumeAnchorAliasWithComment(IList<ParsingEvent> parsingEvents, out AnchorAlias? anchorAlias, out Comment? comment)
		{
			var success = this.TryConsume(parsingEvents, out anchorAlias, out comment);

			return success && comment!.IsInline;
		}

		protected internal virtual bool TryConsumeComment(IList<ParsingEvent> parsingEvents, out Comment? comment)
		{
			var success = this.TryConsume(parsingEvents, out comment);

			return success && comment!.IsInline;
		}

		protected internal virtual bool TryConsumeScalarValue(IList<ParsingEvent> parsingEvents, out Scalar? scalar)
		{
			var success = this.TryConsume(parsingEvents, out scalar);

			return success && !scalar!.IsKey;
		}

		protected internal virtual bool TryConsumeScalarValueWithComment(IList<ParsingEvent> parsingEvents, out Scalar? scalar, out Comment? comment)
		{
			var success = this.TryConsume(parsingEvents, out scalar, out comment);

			return success && !scalar!.IsKey && comment!.IsInline;
		}

		protected internal virtual async Task ValidateAndTrimStreamEvents(IList<ParsingEvent> parsingEvents)
		{
			if(parsingEvents == null)
				throw new ArgumentNullException(nameof(parsingEvents));

			await Task.CompletedTask;

			if(parsingEvents.Count < 1)
				return;

			var valid = true;

			if(parsingEvents[0] is StreamStart)
				parsingEvents.RemoveAt(0);
			else
				valid = false;

			if(valid && parsingEvents.Count < 1)
				valid = false;

			if(valid)
			{
				var lastIndex = parsingEvents.Count - 1;
				if(parsingEvents[lastIndex] is StreamEnd)
					parsingEvents.RemoveAt(lastIndex);
				else
					valid = false;
			}

			if(!valid)
				throw new ArgumentException("The parsing-events must start with a stream-start and end with a stream-end.", nameof(parsingEvents));
		}

		#endregion
	}
}