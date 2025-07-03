using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlTagDirective(TagDirective tag) : YamlDirective<TagDirective>(tag) { }
}