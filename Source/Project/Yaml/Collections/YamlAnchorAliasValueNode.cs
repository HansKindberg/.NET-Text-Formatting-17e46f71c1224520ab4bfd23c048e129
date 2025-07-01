using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlAnchorAliasValueNode(AnchorAlias anchorAlias) : YamlValueNode<AnchorAlias>(anchorAlias)
	{
		#region Methods

		protected internal override string GetAnchorAlias()
		{
			return this.Value.Value.Value;
		}

		#endregion
	}
}