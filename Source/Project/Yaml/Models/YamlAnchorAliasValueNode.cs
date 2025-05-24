using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlAnchorAliasValueNode(AnchorAlias anchorAlias) : YamlValueNode<AnchorAlias>(anchorAlias)
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