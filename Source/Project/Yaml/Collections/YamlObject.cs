using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public abstract class YamlObject
	{
		#region Methods

		protected internal virtual string? GetCommentValue(Comment? comment)
		{
			return comment?.Value.Trim();
		}

		#endregion
	}
}