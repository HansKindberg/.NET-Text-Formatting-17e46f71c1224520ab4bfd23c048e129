using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlScalarKeyAnchorAliasValuePairNode(Scalar key, AnchorAlias value) : YamlScalarKeyValuePairNode<AnchorAlias>(key, value) { }
}