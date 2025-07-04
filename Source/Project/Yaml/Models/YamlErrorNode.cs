using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlErrorNode(Error token) : YamlNode
	{
		#region Properties

		public virtual Error Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion

		#region Methods

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return
			[
				$"{options.ErrorPrefix}{this.Token.Value}"
			];
		}

		#endregion
	}
}