using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlScalarKeyAnchorAliasValuePairNode(Scalar key, AnchorAlias value) : YamlScalarKeyValuePairNode<AnchorAlias>(key, value)
	{
		#region Methods

		protected internal override string GetAnchorAlias()
		{
			return this.Value.Value.Value;
		}

		#endregion
	}
}