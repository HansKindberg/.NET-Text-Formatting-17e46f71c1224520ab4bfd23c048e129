using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlDocument(IYamlDocumentNotation start, IYamlDocumentNotation end) : YamlObject, IYamlDocument
	{
		#region Properties

		public virtual IList<IYamlDirective> Directives { get; } = [];
		public virtual IYamlDocumentNotation End { get; private set; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual bool IncludeEndOnWrite { get; set; }
		public virtual bool IncludeStartOnWrite { get; set; }
		public virtual int Index { get; set; }
		public virtual IList<Comment> LeadingComments { get; } = [];
		IEnumerable<IYamlNode> IYamlDocument.Nodes => this.Nodes;
		public virtual IList<IYamlNode> Nodes { get; } = [];
		public virtual IYamlDocumentNotation Start { get; private set; } = start ?? throw new ArgumentNullException(nameof(start));
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion

		#region Methods

		public virtual async Task Add(IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			await Task.CompletedTask;

			node.Index = this.Nodes.Count;
			node.Parent = null;
			this.Nodes.Add(node);
		}

		public virtual async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			if(namingConvention == null)
				throw new ArgumentNullException(nameof(namingConvention));

			foreach(var node in this.Nodes)
			{
				await node.ApplyNamingConvention(namingConvention);
			}
		}

		public virtual void SetExplicitEnd(Token end, Comment? comment = null)
		{
			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.End = new YamlDocumentEnd(end) { Comment = comment };
		}

		public virtual void SetExplicitStart(Token start, Comment? comment = null)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			this.Start = new YamlDocumentStart(start) { Comment = comment };
		}

		public virtual void SetImplicitEnd(Token end)
		{
			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.End = new YamlDocumentEnd(end.Start, end.End);
		}

		public virtual async Task Sort(IComparer<IYamlNode> comparer)
		{
			if(comparer == null)
				throw new ArgumentNullException(nameof(comparer));

			foreach(var node in this.Nodes)
			{
				await node.Sort(comparer);
			}

			this.Nodes.Sort(comparer);
		}

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var leadingTokens = new SortedDictionary<long, string>();

			foreach(var comment in this.LeadingComments)
			{
				leadingTokens.Add(comment.Start.Line, $"{options.CommentPrefix}{this.GetCommentValue(comment, options)}");
			}

			foreach(var directive in this.Directives)
			{
				leadingTokens.Add(directive.Start.Line, directive.ToString(options));
			}

			foreach(var leadingToken in leadingTokens)
			{
				lines.Add(leadingToken.Value);
			}

			if(this.IncludeStartOnWrite)
				lines.Add(this.Start.ToString(options));

			foreach(var node in this.Nodes)
			{
				await node.Write(lines, options);
			}

			if(this.IncludeEndOnWrite)
				lines.Add(this.End.ToString(options));

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{options.CommentPrefix}{this.GetCommentValue(comment, options)}");
			}
		}

		#endregion
	}
}