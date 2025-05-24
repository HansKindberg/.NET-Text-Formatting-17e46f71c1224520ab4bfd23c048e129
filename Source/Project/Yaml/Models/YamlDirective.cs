using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlDirective : IYamlDirective
	{
		#region Properties

		public virtual Comment? Comment { get; set; }
		public abstract Mark End { get; }
		public abstract Mark Start { get; }

		#endregion
	}
}