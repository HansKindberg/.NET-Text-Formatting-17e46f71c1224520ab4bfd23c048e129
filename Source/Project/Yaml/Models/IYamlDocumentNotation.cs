using YamlDotNet.Core;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDocumentNotation : IYamlComponent
	{
		#region Properties

		Mark End { get; }
		bool Explicit { get; }
		Mark Start { get; }

		#endregion
	}
}