using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public interface IYamlComponent
	{
		#region Properties

		Comment? Comment { get; set; }

		#endregion

		#region Methods

		string ToString(YamlFormatOptions options);

		#endregion
	}
}