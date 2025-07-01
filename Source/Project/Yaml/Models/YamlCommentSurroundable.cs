using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlCommentSurroundable : IYamlCommentSurroundable
	{
		#region Properties

		public virtual IList<Comment> LeadingComments { get; } = [];
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion
	}
}