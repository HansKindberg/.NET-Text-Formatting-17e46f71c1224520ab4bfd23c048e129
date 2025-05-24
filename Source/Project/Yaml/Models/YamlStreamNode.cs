using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlStreamNode(StreamStart start, StreamEnd end) : YamlNode
	{
		#region Properties

		public virtual StreamEnd End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual StreamStart Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

		#endregion
	}
}