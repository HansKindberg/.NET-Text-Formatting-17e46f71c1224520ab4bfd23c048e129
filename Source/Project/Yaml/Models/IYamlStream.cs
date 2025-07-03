using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlStream : IFormatableYaml
	{
		#region Properties

		IList<IYamlDocument> Documents { get; }
		Token End { get; }
		Token Start { get; }

		#endregion
	}
}