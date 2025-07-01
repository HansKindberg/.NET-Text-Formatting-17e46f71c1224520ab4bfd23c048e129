using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public abstract class YamlValueNode<T>(T value) : YamlNode where T : Token
	{
		#region Properties

		public virtual T Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

		#endregion
	}
}