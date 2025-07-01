using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlDocumentNode" />
	public class YamlDocumentNode(IYamlDocumentNotation start, IYamlDocumentNotation end) : YamlNode, IYamlDocumentNode
	{
		#region Properties

		public virtual IList<IYamlDirective> Directives { get; } = [];
		public virtual IYamlDocumentNotation End { get; private set; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual bool IncludeEndOnWrite { get; set; }
		public virtual bool IncludeStartOnWrite { get; set; }
		public virtual IList<Comment> LeadingComments { get; } = [];
		
		////////////////////////////////////////////////////////////////////////////////////////////////public virtual bool Sequence { get; set; }
		
		public virtual IYamlDocumentNotation Start { get; private set; } = start ?? throw new ArgumentNullException(nameof(start));
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion

		#region Methods

		public virtual void SetEnd(Token end, Comment? comment = null)
		{
			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.End = new YamlDocumentEnd(end) { Comment = comment };
		}

		//////////////////////////////////////////public virtual void SetEnd(Mark start, Mark end)
		//////////////////////////////////////////{
		//////////////////////////////////////////	if(start == null)
		//////////////////////////////////////////		throw new ArgumentNullException(nameof(start));

		//////////////////////////////////////////	if(end == null)
		//////////////////////////////////////////		throw new ArgumentNullException(nameof(end));

		//////////////////////////////////////////	this.End = new YamlDocumentEnd(start, end);
		//////////////////////////////////////////}

		public virtual void SetStart(Token start, Comment? comment = null)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			this.Start = new YamlDocumentStart(start) { Comment = comment };
		}

		////////////////////////////////////////////////public virtual void SetStart(Mark start, Mark end)
		////////////////////////////////////////////////{
		////////////////////////////////////////////////	if(start == null)
		////////////////////////////////////////////////		throw new ArgumentNullException(nameof(start));

		////////////////////////////////////////////////	if(end == null)
		////////////////////////////////////////////////		throw new ArgumentNullException(nameof(end));

		////////////////////////////////////////////////	this.Start = new YamlDocumentStart(start, end);
		////////////////////////////////////////////////}

		public override async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var leadingTokens = new SortedDictionary<long, string>();

			foreach(var comment in this.LeadingComments)
			{
				leadingTokens.Add(comment.Start.Line, $"{options.CommentPrefix}{this.GetCommentValue(comment)}");
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

			foreach(var child in this.Children)
			{
				await child.Write(lines, options);
			}

			if(this.IncludeEndOnWrite)
				lines.Add(this.End.ToString(options));

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{options.CommentPrefix}{this.GetCommentValue(comment)}");
			}
		}

		#endregion
	}
}