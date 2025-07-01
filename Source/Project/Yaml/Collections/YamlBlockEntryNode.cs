using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <summary>
	/// Represents the "- " in a sequence.
	/// </summary>
	public class YamlBlockEntryNode(BlockEntry blockEntry) : YamlNode
	{
		#region Properties

		public virtual BlockEntry BlockEntry { get; } = blockEntry ?? throw new ArgumentNullException(nameof(blockEntry));

		#endregion
	}
}