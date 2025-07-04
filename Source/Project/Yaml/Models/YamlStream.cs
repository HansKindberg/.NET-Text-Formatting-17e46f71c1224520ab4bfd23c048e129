using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlStream(Token start, Token end) : IYamlStream
	{
		#region Properties

		public virtual IList<IYamlDocument> Documents { get; } = [];
		public virtual Token End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual Token Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

		#endregion

		#region Methods

		public virtual async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			if(namingConvention == null)
				throw new ArgumentNullException(nameof(namingConvention));

			foreach(var document in this.Documents)
			{
				await document.ApplyNamingConvention(namingConvention);
			}
		}

		protected internal virtual async Task EnsureNecessaryDocumentNotationOnWrite()
		{
			await Task.CompletedTask;

			if(this.Documents.Count < 2)
				return;

			for(var i = 0; i < this.Documents.Count; i++)
			{
				var document = this.Documents[i];

				if(document.IncludeEndOnWrite)
					continue;

				if(document.IncludeStartOnWrite)
					continue;

				if(i < this.Documents.Count - 1 || document.LeadingComments.Any())
					document.IncludeEndOnWrite = true;
			}
		}

		protected internal virtual async Task InitializeDocumentNotationOnWrite()
		{
			await Task.CompletedTask;

			foreach(var document in this.Documents)
			{
				document.IncludeEndOnWrite = document.End.Explicit;
				document.IncludeStartOnWrite = document.Start.Explicit;
			}
		}

		protected internal virtual async Task RemoveUnnecessaryDocumentNotationOnWrite()
		{
			await Task.CompletedTask;

			foreach(var document in this.Documents)
			{
				if(document.IncludeEndOnWrite && document.End.Comment == null)
					document.IncludeEndOnWrite = false;

				if(document is { IncludeStartOnWrite: true, Start.Comment: null } && !document.Directives.Any())
					document.IncludeStartOnWrite = false;
			}
		}

		protected internal virtual async Task ResolveDocumentNotationOnWrite(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await this.InitializeDocumentNotationOnWrite();

			switch(options.DocumentNotation)
			{
				case DocumentNotation.Minimal:
				{
					await this.RemoveUnnecessaryDocumentNotationOnWrite();
					break;
				}
				case DocumentNotation.ForceDocumentEnd:
				case DocumentNotation.ForceDocumentStart:
				case DocumentNotation.ForceDocumentEndAndDocumentStart:
				{
					foreach(var document in this.Documents)
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

			await this.EnsureNecessaryDocumentNotationOnWrite();
		}

		protected internal virtual async Task ResolveForceEndOnWrite(IYamlDocument document, YamlFormatOptions options)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEnd or DocumentNotation.ForceDocumentEndAndDocumentStart)
				document.IncludeEndOnWrite = true;
		}

		protected internal virtual async Task ResolveForceStartOnWrite(IYamlDocument document, YamlFormatOptions options)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			if(options.DocumentNotation is DocumentNotation.ForceDocumentEndAndDocumentStart or DocumentNotation.ForceDocumentStart)
				document.IncludeStartOnWrite = true;
		}

		public virtual async Task Sort(IComparer<IYamlDocument> documentComparer, IComparer<IYamlNode> nodeComparer)
		{
			if(documentComparer == null)
				throw new ArgumentNullException(nameof(documentComparer));

			if(nodeComparer == null)
				throw new ArgumentNullException(nameof(nodeComparer));

			foreach(var document in this.Documents)
			{
				await document.Sort(nodeComparer);
			}

			this.Documents.Sort(documentComparer);
		}

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await this.ResolveDocumentNotationOnWrite(options);

			foreach(var document in this.Documents)
			{
				await document.Write(lines, options);
			}
		}

		#endregion
	}
}