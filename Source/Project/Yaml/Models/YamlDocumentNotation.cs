using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	/// <inheritdoc cref="IYamlDocumentNotation" />
	public abstract class YamlDocumentNotation : IYamlDocumentNotation
	{
		#region Constructors

		protected YamlDocumentNotation(Token token)
		{
			this.Token = token ?? throw new ArgumentNullException(nameof(token));
			this.Start = token.Start;
			this.End = token.End;
			this.Explicit = true;
		}

		protected YamlDocumentNotation(Mark start, Mark end)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.Start = start;
			this.End = end;
			this.Explicit = false;
		}

		#endregion

		#region Properties

		public virtual Comment? Comment { get; set; }
		public virtual Mark End { get; }
		public virtual bool Explicit { get; }
		public virtual Mark Start { get; }
		protected internal virtual Token? Token { get; }

		#endregion

		#region Methods

		protected internal abstract string GetNotation(YamlFormatOptions options);

		#endregion
	}
}