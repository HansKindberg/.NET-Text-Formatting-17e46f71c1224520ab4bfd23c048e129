using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlStreamNode(Token start, Token end) : YamlNode, IYamlRootNode
	{
		#region Properties

		public virtual Token End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual Token Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

		#endregion

		#region Methods

		protected internal virtual async Task EnsureNecessaryDocumentNotationOnWrite(IList<IYamlDocumentNode> documents)
		{
			if(documents == null)
				throw new ArgumentNullException(nameof(documents));

			await Task.CompletedTask;

			if(documents.Count < 2)
				return;

			for(var i = 0; i < documents.Count; i++)
			{
				var document = documents[i];

				if(document.IncludeEndOnWrite)
					continue;

				if(document.IncludeStartOnWrite)
					continue;

				if(i < documents.Count - 1 || document.LeadingComments.Any())
					document.IncludeEndOnWrite = true;
			}
		}

		protected internal virtual async Task<IList<IYamlDocumentNode>> GetDocuments()
		{
			await Task.CompletedTask;

			return [.. this.Children.Cast<IYamlDocumentNode>()];
		}

		protected internal virtual async Task InitializeDocumentNotationOnWrite(IList<IYamlDocumentNode> documents)
		{
			if(documents == null)
				throw new ArgumentNullException(nameof(documents));

			await Task.CompletedTask;

			foreach(var document in documents)
			{
				document.IncludeEndOnWrite = document.End.Explicit;
				document.IncludeStartOnWrite = document.Start.Explicit;
			}
		}

		protected internal virtual async Task RemoveUnnecessaryDocumentNotationOnWrite(IList<IYamlDocumentNode> documents)
		{
			if(documents == null)
				throw new ArgumentNullException(nameof(documents));

			await Task.CompletedTask;

			foreach(var document in documents)
			{
				if(document == null) { }

				////////////////////////if(document.IncludeEndOnWrite && document.EndComment == null)
				////////////////////////	document.IncludeEndOnWrite = false;

				////////////////////////if(document is { IncludeStartOnWrite: true, Comment: null } && !document.Directives.Any())
				////////////////////////	document.IncludeStartOnWrite = false;
			}
		}

		protected internal virtual async Task ResolveDocumentNotationOnWrite(IList<IYamlDocumentNode> documents, YamlFormatOptions options)
		{
			if(documents == null)
				throw new ArgumentNullException(nameof(documents));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await this.InitializeDocumentNotationOnWrite(documents);

			switch(options.DocumentNotation)
			{
				case DocumentNotation.Minimal:
				{
					await this.RemoveUnnecessaryDocumentNotationOnWrite(documents);
					break;
				}
				case DocumentNotation.ForceDocumentEnd:
				case DocumentNotation.ForceDocumentStart:
				case DocumentNotation.ForceDocumentEndAndDocumentStart:
				{
					foreach(var document in documents)
					{
						await this.ResolveForceEndOnWrite(document, options);
						await this.ResolveForceStartOnWrite(document, options);
					}

					break;
				}
				case DocumentNotation.None:
				default:
				{
					break;
				}
			}

			await this.EnsureNecessaryDocumentNotationOnWrite(documents);
		}

		protected internal virtual async Task ResolveForceEndOnWrite(IYamlDocumentNode document, YamlFormatOptions options)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEnd or DocumentNotation.ForceDocumentEndAndDocumentStart)
				document.IncludeEndOnWrite = true;
		}

		protected internal virtual async Task ResolveForceStartOnWrite(IYamlDocumentNode document, YamlFormatOptions options)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEndAndDocumentStart or DocumentNotation.ForceDocumentStart)
				document.IncludeStartOnWrite = true;
		}

		public override async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var documents = await this.GetDocuments();

			await this.ResolveDocumentNotationOnWrite(documents, options);

			await base.Write(lines, options);
		}

		#endregion
	}
}