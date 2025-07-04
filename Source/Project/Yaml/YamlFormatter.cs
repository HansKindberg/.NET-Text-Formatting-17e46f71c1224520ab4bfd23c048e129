using HansKindberg.Text.Formatting.Yaml.Configuration;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Comparison;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class YamlFormatter(IParser<IYamlStream> parser) : ITextFormatter<YamlFormatOptions>
	{
		#region Properties

		protected internal virtual IParser<IYamlStream> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));

		#endregion

		#region Methods

		protected internal virtual async Task ApplyNamingConvention(YamlFormatOptions options, IYamlStream stream)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			if(options.NamingConvention == null)
				return;

			var namingConvention = await this.GetNamingConvention(options.NamingConvention);

			if(namingConvention == null)
				return;

			await stream.ApplyNamingConvention(namingConvention);
		}

		protected internal virtual async Task<IComparer<IYamlDocument>> CreateYamlDocumentComparer(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			return new YamlDocumentComparer(options);
		}

		protected internal virtual async Task<IComparer<IYamlNode>> CreateYamlNodeComparer(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			return new YamlNodeComparer(options);
		}

		public virtual async Task<string> Format(YamlFormatOptions options, string text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var stream = await this.Parser.Parse(text);

			if(!stream.Documents.Any())
				return string.Empty;

			await this.ApplyNamingConvention(options, stream);

			await this.Sort(options, stream);

			var value = await this.GetText(options, stream);

			return value;
		}

		protected internal virtual async Task<INamingConvention?> GetNamingConvention(NamingConvention? namingConvention)
		{
			await Task.CompletedTask;

			if(namingConvention == null)
				return null;

			return namingConvention switch
			{
				NamingConvention.CamelCase => CamelCaseNamingConvention.Instance,
				NamingConvention.Hyphenated => HyphenatedNamingConvention.Instance,
				NamingConvention.LowerCase => LowerCaseNamingConvention.Instance,
				NamingConvention.PascalCase => PascalCaseNamingConvention.Instance,
				NamingConvention.Underscored => UnderscoredNamingConvention.Instance,
				_ => null
			};
		}

		protected internal virtual async Task<string> GetText(YamlFormatOptions options, IYamlStream stream)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			var lines = new List<string>();

			await stream.Write(lines, options);

			return string.Join(options.NewLine, lines).Trim();
		}

		protected internal virtual async Task Sort(YamlFormatOptions options, IYamlStream stream)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			if(!options.DocumentSorting.Enabled && !options.NodeSorting.Enabled && !options.SequenceSorting.Enabled)
				return;

			await Task.CompletedTask;

			var documentComparer = await this.CreateYamlDocumentComparer(options);
			var nodeComparer = await this.CreateYamlNodeComparer(options);

			await stream.Sort(documentComparer, nodeComparer);
		}

		#endregion
	}
}