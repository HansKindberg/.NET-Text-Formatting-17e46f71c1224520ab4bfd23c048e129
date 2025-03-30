using Microsoft.Extensions.Logging;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc />
	public class XmlFormatter : ITextFormatter<IXmlFormat>
	{
		#region Constructors

		public XmlFormatter(ILoggerFactory loggerFactory, IParser<IXmlFragment> parser)
		{
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			this.Logger = loggerFactory.CreateLogger(this.GetType());
			this.Parser = parser ?? throw new ArgumentNullException(nameof(parser));
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual ILoggerFactory LoggerFactory { get; }
		protected internal virtual IParser<IXmlFragment> Parser { get; }

		#endregion

		#region Methods

		public virtual async Task<string> Format(IXmlFormat? options, string? text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var fragment = await this.Parser.Parse(text);
			var xml = new FormattableXml(fragment, this.LoggerFactory, options, this.Parser);

			return await xml.Format();
		}

		#endregion
	}
}