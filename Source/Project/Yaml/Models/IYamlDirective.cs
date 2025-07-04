using YamlDotNet.Core;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDirective : IYamlComponent
	{
		#region Properties

		Mark End { get; }
		Mark Start { get; }

		#endregion
	}
}