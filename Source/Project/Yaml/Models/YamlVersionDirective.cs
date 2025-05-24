using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlVersionDirective(VersionDirective version) : YamlDirective
	{
		#region Properties

		public override Mark End => this.Version.End;
		public override Mark Start => this.Version.Start;
		public virtual VersionDirective Version { get; } = version ?? throw new ArgumentNullException(nameof(version));

		#endregion
	}
}