using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlDocumentEnd : YamlDocumentNotation
	{
		#region Constructors

		public YamlDocumentEnd(Token token) : base(token) { }
		public YamlDocumentEnd(Mark start, Mark end) : base(start, end) { }

		#endregion

		#region Methods

		protected internal override string GetNotation(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return options.EndOfDocument;
		}

		#endregion
	}
}