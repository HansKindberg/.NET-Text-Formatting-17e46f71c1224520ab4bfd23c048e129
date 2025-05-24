using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlScalarKeyAnchorAliasValuePairNode(Scalar key, AnchorAlias value) : YamlScalarKeyValuePairNode<AnchorAlias>(key, value)
	{
		#region Methods

		protected internal override async Task<string?> GetAnchorAlias()
		{
			await Task.CompletedTask;

			return this.Value.Value.Value;
		}

		#endregion
	}
}