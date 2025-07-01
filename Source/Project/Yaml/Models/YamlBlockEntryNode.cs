using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	/// <summary>
	/// A BlockEntry is the "- " prefix for an item in a sequence.
	/// </summary>
	public class YamlBlockEntryNode(BlockEntry token) : YamlStructureNode, IYamlBlockEntryNode
	{
		#region Properties

		public virtual BlockEntry Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}