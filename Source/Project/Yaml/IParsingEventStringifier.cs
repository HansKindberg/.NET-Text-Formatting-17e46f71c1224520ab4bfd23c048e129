using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml
{
	public interface IParsingEventStringifier
	{
		#region Methods

		Task<string?> Stringify(YamlFormatOptions options, ParsingEvent? parsingEvent);

		#endregion
	}
}