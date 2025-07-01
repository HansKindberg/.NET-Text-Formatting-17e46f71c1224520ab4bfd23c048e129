using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlNode" />
	public interface IYamlContentNode : IYamlNode //////////////////////////////////////////////////////////////////////////////////////////////////////, IYamlSequenceNode
	{
		#region Properties

		IList<Comment> LeadingComments { get; }
		IList<Comment> TrailingComments { get; }

		#endregion
	}
}