using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlNode" />
	public interface IYamlRootNode : IYamlNode
	{
		#region Properties

		Token End { get; }
		Token Start { get; }

		#endregion
	}
}