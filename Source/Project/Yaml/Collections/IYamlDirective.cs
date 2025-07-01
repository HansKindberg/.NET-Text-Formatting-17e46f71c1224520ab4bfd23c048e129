using YamlDotNet.Core;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc />
	public interface IYamlDirective : IYamlComponent
	{
		#region Properties

		Mark End { get; }
		Mark Start { get; }

		#endregion
	}
}