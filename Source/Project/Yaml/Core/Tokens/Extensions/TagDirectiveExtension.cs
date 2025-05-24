using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Core.Tokens.Extensions
{
	public static class TagDirectiveExtension
	{
		#region Methods

		public static bool IsDefault(this TagDirective tagDirective)
		{
			if(tagDirective == null)
				throw new ArgumentNullException(nameof(tagDirective));

			return tagDirective.End == Mark.Empty && tagDirective.Start == Mark.Empty;
		}

		#endregion
	}
}