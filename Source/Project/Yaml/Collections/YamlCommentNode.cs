using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlCommentNode(Comment comment) : YamlNode
	{
		#region Properties

		public virtual Comment Comment { get; } = comment ?? throw new ArgumentNullException(nameof(comment));

		#endregion
	}
}