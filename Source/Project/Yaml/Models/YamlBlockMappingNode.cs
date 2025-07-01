using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlBlockMappingNode(BlockMappingStart token) : YamlStructureNode
	{
		#region Properties

		public virtual BlockMappingStart Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}