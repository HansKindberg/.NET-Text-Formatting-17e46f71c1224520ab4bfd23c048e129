using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlContentNode : IYamlNode
	{
		#region Properties

		Anchor? Anchor { get; set; }
		AnchorAlias? AnchorAlias { get; set; }
		Scalar? Key { get; set; }
		Tag? Tag { get; set; }
		Scalar? Value { get; set; }

		#endregion
	}
}