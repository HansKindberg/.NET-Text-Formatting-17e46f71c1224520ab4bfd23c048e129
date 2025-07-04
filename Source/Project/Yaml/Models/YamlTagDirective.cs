using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlTagDirective(TagDirective tag) : YamlDirective<TagDirective>(tag)
	{
		#region Methods

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return
			[
				options.TagDirectivePrefix,
				this.Directive.Handle,
				this.Directive.Prefix
			];
		}

		#endregion
	}
}