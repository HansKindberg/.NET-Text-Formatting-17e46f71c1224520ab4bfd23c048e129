using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDocumentNotation
	{
		#region Properties

		Comment? Comment { get; set; }
		Mark End { get; }
		bool Explicit { get; }
		Mark Start { get; }

		#endregion
	}
}