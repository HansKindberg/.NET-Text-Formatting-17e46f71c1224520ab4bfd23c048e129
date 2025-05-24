using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlDocumentNode(DocumentStart start, DocumentEnd end) : YamlNode, IYamlDocumentNode
	{
		#region Properties

		public virtual IList<IYamlDirective> Directives { get; } = [];
		public virtual DocumentEnd End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual Comment? EndComment { get; set; }
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

		protected internal virtual async Task<int> GetOriginalIndex()
		{
			await Task.CompletedTask;

			var documents = this.Parent!.Children.Cast<IYamlDocumentNode>().ToList();

			documents.Sort((first, second) => first.Start.Start.Line.CompareTo(second.Start.Start.Line));

			return documents.IndexOf(this);
		}

		protected internal virtual async Task<bool> IncludeEndOnWrite(int index, int numberOfSiblings, YamlFormatOptions options, int originalIndex)
		{
			await Task.CompletedTask;

			if(this.EndComment != null)
				return true;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEnd or DocumentNotation.ForceDocumentEndAndDocumentStart)
				return true;

			var isLastChild = this.IsLastChild();

			if(numberOfSiblings > 1 && !isLastChild)
				return true;

			if(index != originalIndex && !isLastChild)
				return true;

			if(index == originalIndex)
			{
				//if(options.DocumentNotation == DocumentNotation.None)
				//	return !this.End.IsImplicit;
			}

			//var first = index == 0;
			//var last = index == this.Parent.Children.Count() - 1;

			//	throw new ArgumentOutOfRangeException(nameof(options.DocumentNotation), options.DocumentNotation, "Document notation must be either 'YAML 1.1' or 'YAML 1.2'.");
			//if(!this.Start.IsImplicit)

			return false;
		}

		protected internal virtual async Task<bool> IncludeStartOnWrite(int index, int numberOfSiblings, YamlFormatOptions options, int originalIndex)
		{
			await Task.CompletedTask;

			if(this.Comment != null)
				return true;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEndAndDocumentStart or DocumentNotation.ForceDocumentStart)
				return true;

			if(this.Directives.Any())
				return true;

			if(options.DocumentNotation == DocumentNotation.None)
			{
				// return !this.Start.IsImplicit;
			}

			return false;

			//var first = index == 0;
			//var last = index == this.Parent.Children.Count() - 1;
		}

		public override async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var index = this.Parent!.Children.IndexOf(this);
			var numberOfSiblings = this.Parent.Children.Count();
			var originalIndex = numberOfSiblings > 1 ? await this.GetOriginalIndex() : index;

			var includeEndOnWrite = await this.IncludeEndOnWrite(index, numberOfSiblings, options, originalIndex);
			var includeStartOnWrite = await this.IncludeStartOnWrite(index, numberOfSiblings, options, originalIndex);

			if(includeStartOnWrite)
				lines.Add(await this.GetDocumentNotation(this.Comment, options.StartOfDocument, options));

			await base.Write(lines, options);

			if(includeEndOnWrite)
				lines.Add(await this.GetDocumentNotation(this.EndComment, options.EndOfDocument, options));
		}

		#endregion
	}
}