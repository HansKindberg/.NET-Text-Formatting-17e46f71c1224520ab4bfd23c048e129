using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlTagDirective(TagDirective tag) : YamlDirective
	{
		#region Properties

		public override Mark End => this.Tag.End;
		public override Mark Start => this.Tag.Start;
		public virtual TagDirective Tag { get; } = tag ?? throw new ArgumentNullException(nameof(tag));

		#endregion
	}
}