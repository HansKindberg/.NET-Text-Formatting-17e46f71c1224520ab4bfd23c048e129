using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlValueNode<T>(T value) : YamlNode where T : ParsingEvent
	{
		#region Properties

		public virtual T Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

		#endregion
	}
}