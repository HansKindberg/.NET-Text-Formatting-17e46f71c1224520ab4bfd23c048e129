using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlCommentSurroundable
	{
		#region Properties

		IList<Comment> LeadingComments { get; }
		IList<Comment> TrailingComments { get; }

		#endregion
	}
}