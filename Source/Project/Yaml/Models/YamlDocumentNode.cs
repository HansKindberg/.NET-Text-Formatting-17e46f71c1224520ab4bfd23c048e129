using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlDocumentNode(DocumentStart start, DocumentEnd end) : YamlNode, IYamlDocumentNode
	{
		#region Properties

		public virtual IList<IYamlDirective> Directives { get; } = [];
		public virtual DocumentEnd End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual Comment? EndComment { get; set; }
		public virtual bool IncludeEndOnWrite { get; set; }
		public virtual bool IncludeStartOnWrite { get; set; }
		public virtual DocumentStart Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

		#endregion

		#region Methods

		protected internal virtual async Task<string> GetDocumentNotation(Comment? comment, string notation, YamlFormatOptions options)
		{
			await Task.CompletedTask;

			var documentNotation = notation ?? throw new ArgumentNullException(nameof(notation));

			if(comment != null)
				documentNotation += $"{options.Space}{options.CommentPrefix}{await this.GetCommentValue(comment)}";

			return documentNotation;
		}

		public override async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			foreach(var comment in this.LeadingComments)
			{
				lines.Add($"{options.CommentPrefix}{await this.GetCommentValue(comment)}");
			}

			if(this.IncludeStartOnWrite)
				lines.Add(await this.GetDocumentNotation(this.Comment, options.StartOfDocument, options));

			foreach(var child in this.Children)
			{
				await child.Write(lines, options);
			}

			if(this.IncludeEndOnWrite)
				lines.Add(await this.GetDocumentNotation(this.EndComment, options.EndOfDocument, options));

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{options.CommentPrefix}{await this.GetCommentValue(comment)}");
			}
		}

		#endregion
	}
}