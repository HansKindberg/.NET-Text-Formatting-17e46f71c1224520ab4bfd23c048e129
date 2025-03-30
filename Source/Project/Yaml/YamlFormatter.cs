using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.RepresentationModel;

namespace HansKindberg.Text.Formatting.Yaml
{
	// TODO: Look over this class.
	///// <inheritdoc />
	public class YamlFormatter(IParser<YamlDocument> parser)
	{
		#region Properties

		protected internal virtual IParser<YamlDocument> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));

		#endregion

		#region Methods

		public virtual async Task<string> Format(YamlFormatOptions options, string text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var yamlDocument = await this.Parser.Parse(text);

			if(options.Sorting.Enabled)
			{
				//yamlDocument = await this.Sort(yamlDocument, options.Sorting);
			}

			return yamlDocument.ToString();
		}

		#endregion
	}
}