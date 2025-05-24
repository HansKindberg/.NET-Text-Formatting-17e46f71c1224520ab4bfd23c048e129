using HansKindberg.Text.Formatting.Yaml.Configuration;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class YamlFormatter(IParser<IYamlNode> parser) : ITextFormatter<YamlFormatOptions>
	{
		#region Properties

		protected internal virtual IParser<IYamlNode> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));

		#endregion

		#region Methods

		protected internal virtual async Task ApplyNamingConvention(IYamlNode node, YamlFormatOptions options)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.NamingConvention == null)
				return;

			var namingConvention = await this.GetNamingConvention(options.NamingConvention);

			if(namingConvention == null)
				return;

			await node.ApplyNamingConvention(namingConvention);
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

			var node = await this.Parser.Parse(text);

			if(!node.Children.Any())
				return string.Empty;

			await this.ApplyNamingConvention(node, options);

			await this.Sort(node, options);

			var value = await this.GetText(node, options);

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

		protected internal virtual async Task<string> GetText(IYamlNode node, YamlFormatOptions options)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var lines = new List<string>();

			await node.Write(lines, options);

			return string.Join(options.NewLine, lines).Trim();
		}

		protected internal virtual async Task Sort(IYamlNode node, YamlFormatOptions options)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(!options.DocumentSorting.Enabled && !options.ScalarSorting.Enabled && !options.SequenceSorting.Enabled)
				return;

			var comparer = await this.CreateYamlNodeComparer(options);

			await node.Sort(comparer);
		}

		#endregion
	}
}