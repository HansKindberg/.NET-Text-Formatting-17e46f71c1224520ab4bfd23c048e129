using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlVersionDirective(VersionDirective version) : YamlDirective<VersionDirective>(version)
	{
		#region Methods

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return
			[
				options.VersionDirectivePrefix,
				$"{this.Directive.Version.Major}.{this.Directive.Version.Minor}"
			];
		}

		#endregion
	}
}