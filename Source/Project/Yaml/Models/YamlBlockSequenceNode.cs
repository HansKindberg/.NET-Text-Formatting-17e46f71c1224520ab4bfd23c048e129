using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlBlockSequenceNode(BlockSequenceStart token) : YamlStructureNode
	{
		#region Properties

		public virtual BlockSequenceStart Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}