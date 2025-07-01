using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlDocumentStart : YamlDocumentNotation
	{
		#region Constructors

		public YamlDocumentStart(Token token) : base(token) { }
		public YamlDocumentStart(Mark start, Mark end) : base(start, end) { }

		#endregion

		#region Methods

		protected internal override string GetNotation(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return options.StartOfDocument;
		}

		#endregion
	}
}