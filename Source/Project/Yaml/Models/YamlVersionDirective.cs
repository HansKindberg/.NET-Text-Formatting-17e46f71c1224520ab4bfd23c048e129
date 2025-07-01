using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlVersionDirective(VersionDirective version) : YamlDirective<VersionDirective>(version) { }
}