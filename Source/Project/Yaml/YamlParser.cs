using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Extensions;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class YamlParser : IParser<IYamlStream>
	{
		#region Properties

		protected internal virtual int InformationalYamlMaximumLength => 100;

		#endregion

		#region Methods

		protected internal virtual async Task AddMissingFlowEntries(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			await Task.CompletedTask;

			for(var i = tokens.Count - 1; i >= 0; i--)
			{
				if(tokens[i] is not FlowSequenceStart flowSequenceStart)
					continue;

				var nextIndex = i + 1;

				if(nextIndex >= tokens.Count)
					continue;

				tokens.Insert(nextIndex, new FlowEntry(flowSequenceStart.Start, flowSequenceStart.End));
			}
		}
















		protected internal virtual async Task<IYamlDocument> CreateDocument(IYamlStream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			var start = await this.CreateDocumentStart(stream);
			var end = await this.CreateDocumentEnd(stream);

			return await this.CreateDocument(start, end);
		}

		protected internal virtual async Task<IYamlDocument> CreateDocument(DocumentEnd implicitStart, IYamlStream stream)
		{
			if(implicitStart == null)
				throw new ArgumentNullException(nameof(implicitStart));

			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			var start = await this.CreateDocumentStart(implicitStart.Start, implicitStart.End);
			var end = await this.CreateDocumentEnd(stream);

			return await this.CreateDocument(start, end);
		}

		protected internal virtual async Task<IYamlDocument> CreateDocument(DocumentStart explicitStart, IYamlStream stream)
		{
			if(explicitStart == null)
				throw new ArgumentNullException(nameof(explicitStart));

			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			var start = await this.CreateDocumentStart(explicitStart);
			var end = await this.CreateDocumentEnd(stream);

			return await this.CreateDocument(start, end);
		}

		protected internal virtual async Task<IYamlDocument> CreateDocument(IYamlDocumentNotation start, IYamlDocumentNotation end)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			if(end == null)
				throw new ArgumentNullException(nameof(end));

			await Task.CompletedTask;

			return new YamlDocument(start, end);
		}



		protected internal virtual async Task<IYamlDocumentNotation> CreateDocumentStart(DocumentStart start)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			await Task.CompletedTask;

			return new YamlDocumentStart(start);
		}
















		protected internal virtual async Task<IDictionary<IYamlDocument, IList<Token>>> CreateDocumentMap(IYamlStream stream, IList<Token> tokens)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			var map = new Dictionary<IYamlDocument, IList<Token>>();

			if(!tokens.Any())
				return map;

			var document = await this.CreateDocument(stream);
			var documentTokens = new List<Token>();
			map.Add(document, documentTokens);

			while(tokens.Count > 0)
			{
				var token = tokens[0];
				tokens.RemoveAt(0);

				switch(token)
				{
					case DocumentEnd end:
					{
						Comment? comment = null;

						// If the next token is an inline comment to the document-end.
						if(this.TryConsumeInlineComment(tokens, out var inlineComment))
							comment = inlineComment;

						document.SetExplicitEnd(end, comment);

						// If there are more tokens that are not comments.
						if(tokens.Any(item => item is not Comment))
						{
							document = await this.CreateDocument(end, stream);
							documentTokens = [];
							map.Add(document, documentTokens);
						}

						break;
					}
					case DocumentStart start:
					{
						Comment? comment = null;

						// If the next token is an inline comment to the document-start.
						if(this.TryConsumeInlineComment(tokens, out var inlineComment))
							comment = inlineComment;

						var documentHasContentTokens = documentTokens.Any(item => item is not Comment and not TagDirective and not VersionDirective);

						if(document.Start.Explicit || document.End.Explicit || documentHasContentTokens)
						{
							if(!document.End.Explicit)
								document.SetImplicitEnd(start);

							document = await this.CreateDocument(start, stream);
							document.Start.Comment = comment;
							documentTokens = [];
							map.Add(document, documentTokens);
						}
						else
						{
							document.SetExplicitStart(start, comment);
						}

						break;
					}
					default:
					{
						documentTokens.Add(token);

						break;
					}
				}
			}

			// Move certain trailing comments to the next document as leading comments.
			for(var i = 0; i < map.Count - 1; i++)
			{
				var mapping = map.ElementAt(i);

				if(!mapping.Key.Start.Explicit)
					continue;

				if(mapping.Key.End.Explicit)
					continue;

				var tokensAfterStart = mapping.Value.Where(token => token.Start.Line > mapping.Key.Start.Start.Line).Reverse().ToList();

				foreach(var token in tokensAfterStart)
				{
					if(token is not Comment { IsInline: false })
						break;

					mapping.Value.Remove(token);
					map.ElementAt(i + 1).Value.Insert(0, token);
				}
			}

			return map;
		}


















		protected internal virtual async Task BuildStream(IYamlStream stream, IList<Token> tokens)
		{
			var map = await this.CreateDocumentMap(stream, tokens);

			//if(!tokens.Any())
			//	return;

			//var documentMap = new List<KeyValuePair<IYamlDocument, IList<Token>>>
			//{
			//	await this.CreateDocumentMapping(stream)
			//};

			//for(var i = 0; i < tokens.Count; i++)
			//{
			//	var token = tokens[i];
			//	var documentMapping = documentMap.Last();
			//	var document = documentMapping.Key;
			//	var mapping = documentMapping.Value;

			//	switch(token)
			//	{
			//		case DocumentEnd end:
			//		{
			//			Comment? comment = null;

			//			// If the next token is an inline comment to the document-end.
			//			if(this.TryGetInlineComment(i, end, tokens, out var inlineComment))
			//			{
			//				comment = inlineComment;
			//				i++;
			//			}

			//			document.SetEnd(end, comment);

			//			// If there are more tokens.
			//			if(i < tokens.Count - 1)
			//			{
			//				documentMapping = await this.CreateDocumentMapping(end, stream);
			//				documentMap.Add(documentMapping);
			//			}

			//			break;
			//		}
			//		case DocumentStart start:
			//		{
			//			Comment? comment = null;

			//			// If the next token is an inline comment to the document-start.
			//			if(this.TryGetInlineComment(i, start, tokens, out var inlineComment))
			//			{
			//				comment = inlineComment;
			//				i++;
			//			}

			//			var hasDataTokens = mapping.Any(item => item is not Comment and not TagDirective and not VersionDirective);

			//			if(document.Start.Explicit || document.End.Explicit || hasDataTokens)
			//			{
			//				if(!document.End.Explicit)
			//					document.SetEnd(start);

			//				documentMapping = await this.CreateDocumentMapping(start, stream);
			//				documentMapping.Key.Start.Comment = comment;
			//				documentMap.Add(documentMapping);
			//			}
			//			else
			//			{
			//				document.SetStart(start, comment);
			//			}

			//			break;
			//		}
			//		default:
			//		{
			//			mapping.Add(token);

			//			break;
			//		}
			//	}
			//}

			foreach(var mapping in map)
			{
				await this.PopulateDocument(mapping.Key, mapping.Value);
				stream.Documents.Add(mapping.Key);
			}
		}

		protected internal virtual async Task CorrectTokens(IList<Token> tokens)
		{
			await this.AddMissingFlowEntries(tokens);
		}

		protected internal virtual async Task<IYamlNode> CreateBlockEntryNode(BlockEntry token)
		{
			await Task.CompletedTask;

			return new YamlBlockEntryNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateBlockMappingNode(BlockMappingStart token)
		{
			await Task.CompletedTask;

			return new YamlBlockMappingNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateBlockSequenceNode(BlockSequenceStart token)
		{
			await Task.CompletedTask;

			return new YamlBlockSequenceNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateContentNode(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			if(!tokens.Any())
				throw new ArgumentException("The tokens can not be empty.", nameof(tokens));

			await Task.CompletedTask;

			var node = new YamlContentNode();

			// The key-token is not necessary. We use Scalar.IsKey to determine if a scalar is a key.
			if(tokens[0] is Key)
				tokens.RemoveAt(0);

			// The value-token is not necessary. We use Scalar.IsKey to determine if a scalar is a value.
			var value = tokens.FirstOrDefault(token => token is Value);
			if(value != null)
				tokens.Remove(value);

			// First
			if(tokens.FirstOrDefault() is Scalar { IsKey: true } keyScalar)
			{
				node.Key = keyScalar;
				tokens.RemoveAt(0);
			}

			// Last
			if(tokens.LastOrDefault() is Comment { IsInline: true } comment)
			{
				node.Comment = comment;
				tokens.Remove(comment);
			}

			var anchor = tokens.OfType<Anchor>().FirstOrDefault();
			if(anchor != null)
			{
				node.Anchor = anchor;
				tokens.Remove(anchor);
			}

			var anchorAlias = tokens.OfType<AnchorAlias>().FirstOrDefault();
			if(anchorAlias != null)
			{
				node.AnchorAlias = anchorAlias;
				tokens.Remove(anchorAlias);
			}

			var tag = tokens.OfType<Tag>().FirstOrDefault();
			if(tag != null)
			{
				node.Tag = tag;
				tokens.Remove(tag);
			}

			var valueScalar = tokens.OfType<Scalar>().FirstOrDefault(scalar => !scalar.IsKey);
			if(valueScalar != null)
			{
				node.Value = valueScalar;
				tokens.Remove(valueScalar);
			}

			if(tokens.Any())
				throw new InvalidOperationException("Invalid tokens for creating a yaml-content-node.");

			return node;
		}



		protected internal virtual async Task<IYamlDocumentNotation> CreateDocumentEnd(IYamlStream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			return await this.CreateDocumentEnd(stream.End.Start, stream.End.End);
		}

		protected internal virtual async Task<IYamlDocumentNotation> CreateDocumentEnd(Mark start, Mark end)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			if(end == null)
				throw new ArgumentNullException(nameof(end));

			await Task.CompletedTask;

			return new YamlDocumentEnd(start, end);
		}

		protected internal virtual async Task<IYamlDocumentNotation> CreateDocumentStart(IYamlStream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			return await this.CreateDocumentStart(stream.Start.Start, stream.Start.End);
		}

		protected internal virtual async Task<IYamlDocumentNotation> CreateDocumentStart(Mark start, Mark end)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			if(end == null)
				throw new ArgumentNullException(nameof(end));

			await Task.CompletedTask;

			return new YamlDocumentStart(start, end);
		}

		protected internal virtual async Task<IYamlNode> CreateErrorNode(Error token)
		{
			await Task.CompletedTask;

			return new YamlErrorNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateFlowEntryNode(FlowEntry token)
		{
			await Task.CompletedTask;

			return new YamlFlowEntryNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateFlowMappingNode(FlowMappingStart token)
		{
			await Task.CompletedTask;

			return new YamlFlowMappingNode(token);
		}

		protected internal virtual async Task<IYamlNode> CreateFlowSequenceNode(FlowSequenceStart token)
		{
			await Task.CompletedTask;

			return new YamlFlowSequenceNode(token);
		}

		protected internal virtual async Task<IScanner> CreateScanner(TextReader textReader)
		{
			await Task.CompletedTask;

			return new Scanner(textReader, false);
		}

		protected internal virtual async Task<IYamlStream> CreateStream(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			if(tokens.Count < 2)
				throw new ArgumentException("The tokens must contain at least a stream-start and a stream-end.", nameof(tokens));

			if(tokens[0] is not StreamStart streamStart)
				throw new ArgumentException("The first token must be a stream-start.", nameof(tokens));

			if(tokens.Last() is not StreamEnd streamEnd)
				throw new ArgumentException("The last token must be a stream-end.", nameof(tokens));

			tokens.Remove(streamStart);
			tokens.Remove(streamEnd);

			var stream = await this.CreateStream(streamStart, streamEnd);

			await this.BuildStream(stream, tokens);

			return stream;
		}

		protected internal virtual async Task<IYamlStream> CreateStream(Token start, Token end)
		{
			await Task.CompletedTask;

			return new YamlStream(start, end);
		}

		protected internal virtual async Task<IYamlDirective> CreateTagDirective(TagDirective tagDirective)
		{
			if(tagDirective == null)
				throw new ArgumentNullException(nameof(tagDirective));

			await Task.CompletedTask;

			return new YamlTagDirective(tagDirective);
		}

		protected internal virtual async Task<IYamlDirective> CreateVersionDirective(VersionDirective versionDirective)
		{
			if(versionDirective == null)
				throw new ArgumentNullException(nameof(versionDirective));

			await Task.CompletedTask;

			return new YamlVersionDirective(versionDirective);
		}

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value != null && value.Length > this.InformationalYamlMaximumLength)
				value = $"{value.Substring(0, this.InformationalYamlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<IYamlStream> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				var tokens = await this.ParseToTokens(value);

				var stream = await this.CreateStream(tokens);

				return stream;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-node.", exception);
			}
		}

		protected internal virtual async Task<IList<Token>> ParseToTokens(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			var tokens = new List<Token>();

			using(var stringReader = new StringReader(value))
			{
				var scanner = await this.CreateScanner(stringReader);

				while(scanner.MoveNext())
				{
					tokens.Add(scanner.Current!);
				}
			}

			await this.CorrectTokens(tokens);

			await this.SortTokens(tokens);

			await this.ResolveInlineComments(tokens);

			return tokens;
		}

		protected internal virtual async Task PopulateDocument(IYamlDocument document, IList<Token> tokens)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			await this.PopulateDocumentLeadingTokens(document, tokens);
			await this.PopulateDocumentTrailingComments(document, tokens);
			await this.PopulateDocumentTree(document, tokens);
		}

		protected internal virtual async Task PopulateDocumentLeadingTokens(IYamlDocument document, IList<Token> tokens)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			await Task.CompletedTask;

			var leadingTokens = tokens.Where(token => token.Start.Line < document.Start.Start.Line).ToList();

			while(leadingTokens.Count > 0)
			{
				var leadingToken = leadingTokens[0];
				leadingTokens.RemoveAt(0);
				tokens.Remove(leadingToken);

				switch(leadingToken)
				{
					case Comment { IsInline: true }:
					{
						throw new InvalidOperationException($"The leading token {leadingToken} can not be an inline comment.");
					}
					case Comment comment:
					{
						document.LeadingComments.Add(comment);

						break;
					}
					case TagDirective or VersionDirective:
					{
						var directive = leadingToken is TagDirective tagDirective ? await this.CreateTagDirective(tagDirective) : await this.CreateVersionDirective((VersionDirective)leadingToken);

						if(this.TryConsumeInlineComment(leadingTokens, out var comment))
						{
							directive.Comment = comment;
							tokens.Remove(comment!);
						}

						document.Directives.Add(directive);

						break;
					}
					default:
					{
						throw new InvalidOperationException($"The leading token {leadingToken} must be a comment, tag-directive or version-directive.");
					}
				}
			}
		}

		protected internal virtual async Task PopulateDocumentTrailingComments(IYamlDocument document, IList<Token> tokens)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			await Task.CompletedTask;

			var trailingTokens = tokens.Where(token => token.Start.Line > document.End.Start.Line).ToList();

			foreach(var trailingToken in trailingTokens)
			{
				if(trailingToken is not Comment comment)
					throw new InvalidOperationException($"The trailing token {trailingToken} must be a comment.");

				if(comment.IsInline)
					throw new InvalidOperationException($"The trailing token {trailingToken} can not be an inline comment.");

				document.TrailingComments.Add(comment);

				tokens.Remove(trailingToken);
			}
		}

		protected internal virtual async Task PopulateDocumentTree(IYamlDocument document, IList<Token> tokens)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			if(!tokens.Any())
				return;

			var comments = new List<Comment>();
			var root = new YamlRootNode();
			IYamlNode parent = root;

			while(tokens.Count > 0)
			{
				var token = tokens[0];
				tokens.RemoveAt(0);

				switch(token)
				{
					case BlockEnd:
					case FlowMappingEnd:
					case FlowSequenceEnd:
					{
						parent = parent is IYamlBlockEntryNode or IYamlFlowEntryNode ? parent.Parent!.Parent! : parent.Parent!;

						break;
					}
					case BlockEntry blockEntry:
					{
						if(parent is IYamlBlockEntryNode)
							parent = parent.Parent!;

						var node = await this.CreateBlockEntryNode(blockEntry);
						await parent.Add(node);
						parent = node;

						break;
					}





















					case BlockMappingStart blockMappingStart:
					{
						var node = await this.CreateBlockMappingNode(blockMappingStart);
						await parent.Add(node);
						parent = node;

						break;
					}






















					case BlockSequenceStart blockSequenceStart:
					{
						var node = await this.CreateBlockSequenceNode(blockSequenceStart);
						await parent.Add(node);
						parent = node;

						break;
					}
					case Comment comment:
					{
						if(comment.IsInline)
						{
#pragma warning disable IDE0045 // Convert to conditional expression
							// If the yaml contains flow (json) syntax, especially on the same line, inline comments can be left alone and must be handled afterward.
							if(root.Descendants().LastOrDefault() is IYamlContentNode { Comment: null } lastContentNode)
								lastContentNode.Comment = comment;
							else
								throw new InvalidOperationException("Inline comment should have been handled earlier.");
#pragma warning restore IDE0045 // Convert to conditional expression
						}
						else
						{
							comments.Add(comment);
						}

						break;
					}
					case Error error:
					{
						await parent.Add(await this.CreateErrorNode(error));

						break;
					}
					case FlowEntry flowEntry:
					{
						if(parent is IYamlFlowEntryNode)
							parent = parent.Parent!;

						var node = await this.CreateFlowEntryNode(flowEntry);
						await parent.Add(node);
						parent = node;

						break;
					}
					case FlowMappingStart flowMappingStart:
					{
						var node = await this.CreateFlowMappingNode(flowMappingStart);
						await parent.Add(node);
						parent = node;

						break;
					}
					case FlowSequenceStart flowSequenceStart:
					{
						var node = await this.CreateFlowSequenceNode(flowSequenceStart);
						await parent.Add(node);
						parent = node;

						break;
					}
					case Anchor:
					case AnchorAlias:
					case Key:
					case Scalar:
					case Tag:
					case Value:
					{
						var followingTokensOnSameLine = tokens.Where(item => item.Start.Line == token.Start.Line).ToList();

						var relevantTokensOnSameLine = new List<Token> { token };

						foreach(var relevantTokenOnSameLine in followingTokensOnSameLine)
						{
							if(relevantTokenOnSameLine is not (Anchor or AnchorAlias or Comment or Key or Scalar or Tag or Value))
								break;

							relevantTokensOnSameLine.Add(relevantTokenOnSameLine);
							tokens.Remove(relevantTokenOnSameLine);
						}

						var node = await this.CreateContentNode(relevantTokensOnSameLine);

						node.LeadingComments.AddRange(comments);
						comments.Clear();

						await parent.Add(node);

						break;
					}
					default:
					{
						throw new InvalidOperationException($"Invalid token: {token.GetType()}");
					}
				}
			}

			if(comments.Any())
			{
				var container = root.Descendants().LastOrDefault() as IYamlCommentSurroundable ?? document;

				container.TrailingComments.AddRange(comments);
				comments.Clear();
			}

			foreach(var child in root.Children)
			{
				await document.Add(child);
			}
		}

		/// <summary>
		/// Make inline comments, that are not, to inline comments.
		/// </summary>
		protected internal virtual async Task ResolveInlineComments(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			await Task.CompletedTask;

			var standaloneComments = tokens.OfType<Comment>().Where(comment => !comment.IsInline).ToList();

			var inlineCommentsToResolve = new Dictionary<Comment, Comment>();

			foreach(var standaloneComment in standaloneComments)
			{
				if(!tokens.Any(token => token is not Comment && token.Start.Line == standaloneComment.Start.Line && token.Start.Column < standaloneComment.Start.Column))
					continue;

				inlineCommentsToResolve.Add(standaloneComment, new Comment(standaloneComment.Value, true, standaloneComment.Start, standaloneComment.End));
			}

			foreach(var entry in inlineCommentsToResolve)
			{
				var index = tokens.IndexOf(entry.Key);
				tokens[index] = entry.Value;
			}
		}

		protected internal virtual async Task SortTokens(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			await Task.CompletedTask;

			// We need a stable sort. That is, the original order of the tokens must not change if they are considered equal.
			// Linq's OrderBy has a stable sort, List<T>.Sort() has not.
			var sortedTokens = tokens.OrderBy(token => token.Start.Line).ThenBy(token => token.Start.Column).ToList();
			tokens.Clear();
			tokens.AddRange(sortedTokens);
		}

		protected internal virtual bool TryConsumeInlineComment(IList<Token> tokens, out Comment? comment)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			if(tokens.Any() && tokens[0] is Comment { IsInline: true } inlineComment)
			{
				comment = inlineComment;
				tokens.RemoveAt(0);
				return true;
			}

			comment = null;

			return false;
		}

		#endregion



















		//protected internal virtual bool TryGetInlineComment(int index, Token token, IList<Token> tokens, out Comment? comment)
		//{
		//	if(token == null)
		//		throw new ArgumentNullException(nameof(token));

		//	if(tokens == null)
		//		throw new ArgumentNullException(nameof(tokens));

		//	var nextIndex = index + 1;

		//	if(nextIndex < tokens.Count && tokens[nextIndex] is Comment { IsInline: true } inlineComment && inlineComment.Start.Line == token.Start.Line && inlineComment.Start.Column > token.Start.Column)
		//	{
		//		comment = inlineComment;
		//		return true;
		//	}

		//	comment = null;

		//	return false;
		//}

		////////////////////protected internal virtual async Task<KeyValuePair<IYamlDocument, IList<Token>>> CreateDocumentMapping(IYamlStream stream)
		////////////////////{
		////////////////////	var document = await this.CreateDocument(stream);

		////////////////////	return await this.CreateDocumentMapping(document);
		////////////////////}

		////////////////////protected internal virtual async Task<KeyValuePair<IYamlDocument, IList<Token>>> CreateDocumentMapping(Token documentStart, IYamlStream stream)
		////////////////////{
		////////////////////	var document = await this.CreateDocument(documentStart, stream);

		////////////////////	return await this.CreateDocumentMapping(document);
		////////////////////}

		////////////////////protected internal virtual async Task<KeyValuePair<IYamlDocument, IList<Token>>> CreateDocumentMapping(IYamlDocument document)
		////////////////////{
		////////////////////	await Task.CompletedTask;

		////////////////////	return new KeyValuePair<IYamlDocument, IList<Token>>(document, []);
		////////////////////}
	}
}