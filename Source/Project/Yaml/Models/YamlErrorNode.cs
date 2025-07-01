using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlErrorNode(Error token) : YamlNode
	{
		#region Properties

		public virtual Error Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}