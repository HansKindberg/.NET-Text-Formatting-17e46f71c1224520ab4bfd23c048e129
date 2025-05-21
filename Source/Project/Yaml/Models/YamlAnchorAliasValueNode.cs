using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlAnchorAliasValueNode(AnchorAlias anchorAlias) : YamlValueNode<AnchorAlias>(anchorAlias) { }
}