using HansKindberg.Text.Formatting.Yaml.Configuration;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HansKindberg.Text.Formatting.Yaml
{
	///// <inheritdoc />
	public class YamlFormatter(IParser<IList<YamlNode>> parser)
	{
		#region Properties

		protected internal virtual string NewLine => Environment.NewLine;
		protected internal virtual IParser<IList<YamlNode>> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));
		protected internal virtual string StartOfDocument => "---";

		#endregion

		#region Methods

		protected internal virtual async Task<ISerializer> CreateSerializer(YamlFormatOptions options)
		{
			return new SerializerBuilder()
				.WithNamingConvention(await this.GetNamingConvention(options.NamingConvention))
				//.WithNewLine(this.NewLine)
				.Build();
		}

		public virtual async Task<string> Format(YamlFormatOptions options, string text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var yamlNodes = await this.Parser.Parse(text);

			var result = string.Empty;

			if(yamlNodes.Count <= 0)
				return result;

			if(options.Sorting.Enabled)
			{
				//yamlDocument = await this.Sort(yamlDocument, options.Sorting);
			}

			var serializer = await this.CreateSerializer(options);

			var parts = yamlNodes.Select(serializer.Serialize).ToList();

			result = (parts.Count == 1 ? parts[0] : parts.Aggregate(result, (current, part) => current + $"{this.StartOfDocument}{this.NewLine}{part}")).Trim();

			return result;
		}

		protected internal virtual async Task<INamingConvention> GetNamingConvention(NamingConvention? namingConvention)
		{
			await Task.CompletedTask;

			return namingConvention switch
			{
				NamingConvention.CamelCase => CamelCaseNamingConvention.Instance,
				NamingConvention.Hyphenated => HyphenatedNamingConvention.Instance,
				NamingConvention.LowerCase => LowerCaseNamingConvention.Instance,
				NamingConvention.PascalCase => PascalCaseNamingConvention.Instance,
				NamingConvention.Underscored => UnderscoredNamingConvention.Instance,
				_ => NullNamingConvention.Instance
			};
		}

		#endregion
	}
}