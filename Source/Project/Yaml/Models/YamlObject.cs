using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlObject
	{
		#region Methods

		protected internal virtual string? GetCommentValue(Comment? comment, YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var value = comment?.Value;

			if(value != null && options.TrimComments)
				value = value.Trim();

			return value;
		}

		#endregion
	}
}