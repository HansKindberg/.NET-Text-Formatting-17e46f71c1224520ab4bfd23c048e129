using System.Xml;
using HansKindberg.Text.Formatting.Extensions;
using HtmlAgilityPack;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc />
	public class XmlParser : IParser<IXmlFragment>
	{
		#region Fields

		private const int _informationalXmlMaximumLength = 100;

		#endregion

		#region Methods

		protected internal virtual async Task<HtmlDocument> CreateHtmlDocument()
		{
			await Task.CompletedTask;

			HtmlNode.ElementsFlags.Clear();

			return new HtmlDocument
			{
				BackwardCompatibility = false,
				OptionOutputAsXml = true,
				OptionPreserveXmlNamespaces = true,
				OptionXmlForceOriginalComment = true
			};
		}

		protected internal virtual async Task<TextReader> CreateTextReader(string value)
		{
			await Task.CompletedTask;

			return new StringReader(value);
		}

		protected internal virtual async Task<XmlDocument> CreateXmlDocument(XmlNameTable xmlNameTable)
		{
			await Task.CompletedTask;

			return new XmlDocument(xmlNameTable)
			{
				PreserveWhitespace = true
			};
		}

		protected internal virtual async Task<IXmlFragment> CreateXmlFragment(IList<XmlNode> xmlNodes)
		{
			if(xmlNodes == null)
				throw new ArgumentNullException(nameof(xmlNodes));

			await Task.CompletedTask;

			var xmlFragment = new XmlFragment();

			foreach(var xmlNode in xmlNodes)
			{
				xmlFragment.Nodes.Add(xmlNode);
			}

			return xmlFragment;
		}

		protected internal virtual async Task<XmlNamespaceManager> CreateXmlNamespaceManager(XmlNameTable xmlNameTable)
		{
			await Task.CompletedTask;

			return new PermissiveXmlNamespaceManager(xmlNameTable);
		}

		protected internal virtual async Task<XmlNameTable> CreateXmlNameTable()
		{
			await Task.CompletedTask;

			return new NameTable();
		}

		protected internal virtual async Task<IList<XmlNode>> CreateXmlNodes(IList<HtmlNode> htmlNodes)
		{
			if(htmlNodes == null)
				throw new ArgumentNullException(nameof(htmlNodes));

			await Task.CompletedTask;

			var xmlNodes = new List<XmlNode>();

			var xmlNameTable = await this.CreateXmlNameTable();

			foreach(var htmlNode in htmlNodes)
			{
				var xmlDocument = await this.CreateXmlDocument(xmlNameTable);

				using(var textReader = await this.CreateTextReader(htmlNode.OuterHtml))
				{
					using(var xmlReader = await this.CreateXmlReader(await this.CreateXmlParserContext(xmlNameTable), await this.CreateXmlReaderSettings(), textReader))
					{
						xmlDocument.Load(xmlReader);

						if(xmlDocument.ChildNodes.Count != 1)
							throw new InvalidOperationException($"The xml-document should only contain one root-node. The xml-document contains {xmlDocument.ChildNodes.Count} root-nodes.");

						xmlNodes.Add(xmlDocument.ChildNodes[0]);
					}
				}
			}

			return xmlNodes;
		}

		protected internal virtual async Task<XmlParserContext> CreateXmlParserContext(XmlNameTable xmlNameTable)
		{
			await Task.CompletedTask;

			return new XmlParserContext(xmlNameTable, await this.CreateXmlNamespaceManager(xmlNameTable), null, XmlSpace.Preserve);
		}

		protected internal virtual async Task<XmlReader> CreateXmlReader(XmlParserContext parserContext, XmlReaderSettings settings, TextReader textReader)
		{
			if(parserContext == null)
				throw new ArgumentNullException(nameof(parserContext));

			if(settings == null)
				throw new ArgumentNullException(nameof(settings));

			if(textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			await Task.CompletedTask;

			return XmlReader.Create(textReader, settings, parserContext);
		}

		protected internal virtual async Task<XmlReaderSettings> CreateXmlReaderSettings()
		{
			await Task.CompletedTask;

			return new XmlReaderSettings
			{
				CheckCharacters = false,
				ConformanceLevel = ConformanceLevel.Auto
			};
		}

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value is { Length: > _informationalXmlMaximumLength })
				value = $"{value.Substring(0, _informationalXmlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<IXmlFragment> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				var repairedValue = await this.Repair(value);

				var htmlDocument = await this.CreateHtmlDocument();

				htmlDocument.LoadHtml(repairedValue);

				var xmlNodes = await this.CreateXmlNodes(htmlDocument.DocumentNode.ChildNodes);

				var xmlFragment = await this.CreateXmlFragment(xmlNodes);

				return xmlFragment;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to xml-fragment.", exception);
			}
		}

		/// <summary>
		/// Repairs the value to parse with the help of HtmlAgilityPack.
		/// </summary>
		protected internal virtual async Task<string> Repair(string value)
		{
			var htmlDocument = await this.CreateHtmlDocument();

			htmlDocument.LoadHtml(value);

			// If we have this for example: "<div><p>", we get this back: "<div><p></p></div>".
			var repairedValue = htmlDocument.DocumentNode.OuterHtml;

			if(!string.IsNullOrEmpty(repairedValue) && htmlDocument.OptionOutputAsXml)
			{
				// When OptionOutputAsXml = true, HtmlAgilityPack adds a <?xml version="1.0" encoding="{SOME_ENCODING}"?> tag at the beginning of the document so we have to remove it.
				var index = repairedValue.IndexOf("\"?>", StringComparison.OrdinalIgnoreCase);

				if(index > -1)
					repairedValue = repairedValue.Substring(index + 3);
			}

			return repairedValue;
		}

		#endregion
	}
}