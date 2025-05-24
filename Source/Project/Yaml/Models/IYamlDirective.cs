using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDirective
	{
		#region Properties

		Comment? Comment { get; set; }
		Mark End { get; }
		Mark Start { get; }

		#endregion
	}
}