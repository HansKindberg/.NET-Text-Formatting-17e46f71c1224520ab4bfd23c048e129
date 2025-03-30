using System.Diagnostics.CodeAnalysis;
using System.Xml;
using HansKindberg.Text.Formatting.Logging.Extensions;
using HansKindberg.Text.Formatting.Xml.Comparison;
using Microsoft.Extensions.Logging;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc />
	public class FormattableXml : IFormattableXml
	{
		#region Fields

		private static readonly IXmlFormat _commentOptions = new XmlFormat
		{
			Attribute =
			{
				SortAlphabetically = false
			}
		};

		#endregion

		#region Constructors

		public FormattableXml(IXmlFragment fragment, ILoggerFactory loggerFactory, IXmlFormat options, IParser<IXmlFragment> parser)
		{
			this.Fragment = fragment ?? throw new ArgumentNullException(nameof(fragment));
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			this.Logger = loggerFactory.CreateLogger(this.GetType());
			this.Options = options ?? throw new ArgumentNullException(nameof(options));
			this.Parser = parser ?? throw new ArgumentNullException(nameof(parser));
		}

		#endregion

		#region Properties

		protected internal virtual IXmlFormat CommentOptions => _commentOptions;
		protected internal virtual IXmlFragment Fragment { get; }
		protected internal virtual ILogger Logger { get; }
		protected internal virtual ILoggerFactory LoggerFactory { get; }
		protected internal virtual IXmlFormat Options { get; }
		protected internal virtual IParser<IXmlFragment> Parser { get; }

		#endregion

		#region Methods

		protected internal virtual IEnumerable<XmlNode> Descendants()
		{
			foreach(var node in this.Fragment.Nodes)
			{
				yield return node;

				foreach(var descendant in this.Descendants(node))
				{
					yield return descendant;
				}
			}
		}

		protected internal virtual IEnumerable<XmlNode> Descendants(XmlNode node)
		{
			if(node == null)
				yield break;

			foreach(XmlNode child in node.ChildNodes)
			{
				yield return child;

				foreach(var descendant in this.Descendants(child))
				{
					yield return descendant;
				}
			}
		}

		public virtual async Task<string> Format()
		{
			await this.FormatAttributes();
			await this.FormatComments();
			await this.FormatElements();

			//var xmlWriterSettings = new XmlWriterSettings
			//{
			//	ConformanceLevel = ConformanceLevel.Fragment,
			//	DoNotEscapeUriAttributes = true,
			//	Indent = this.Options.Indent,
			//	IndentChars = this.Options.IndentString,
			//	NewLineChars = Environment.NewLine,
			//	NewLineHandling = NewLineHandling.Replace,
			//	NewLineOnAttributes = true,
			//	CheckCharacters = false
			//};

			return "";
			//using(var xmlWriter = XmlWriter.Create(output, xmlWriterSettings))
			//{
			//	foreach(var node in this.Nodes)
			//	{
			//		node.WriteTo(xmlWriter);
			//	}
			//}
		}

		protected internal virtual async Task FormatAttributes()
		{
			await Task.CompletedTask;

			foreach(var descendant in this.Descendants())
			{
				if(descendant.Attributes == null || descendant.Attributes.Count == 0)
					continue;

				var attributes = descendant.Attributes.Cast<XmlAttribute>().Select((attribute, index) => new XmlAttributeWrapper(this.Logger, attribute) { InitialIndex = index }).ToList();
				var comparer = new XmlAttributeComparer(this.Options.Attribute);
				attributes.Sort(comparer);

				descendant.Attributes.RemoveAll();

				foreach(var attribute in attributes)
				{
					if(attribute.Value == "urn:schemas-microsoft-com:asm.v1")
					{
						continue;
					}

					descendant.Attributes.Append(attribute.XmlNode);
				}
			}
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		protected internal virtual async Task FormatComments()
		{
			if(this.Options.Comment.Omit || this.Options.Comment.Mode == CommentMode.None)
				return;

			foreach(var comment in this.Descendants().OfType<XmlComment>())
			{
				var value = comment.Value.Trim();

				value = value.Replace(Environment.NewLine, " ");
				value = value.Replace("\n", " ");
				value = value.Replace("\r", " ");
				value = value.Replace("\t", " ");

				while(value.IndexOf("  ", StringComparison.Ordinal) >= 0)
				{
					value = value.Replace("  ", " ");
				}

				var unTrim = true;

				if(this.Options.Comment.Mode == CommentMode.SingleLineOrAsXml)
				{
					try
					{
						var fragment = await this.Parser.Parse(value);

						if(fragment.Nodes.Any() && (fragment.Nodes.Count > 1 || fragment.Nodes[0] is not XmlText))
						{
							var formattableXml = new FormattableXml(fragment, this.LoggerFactory, this.CommentOptions, this.Parser);
							value = await formattableXml.Format();

							if(fragment.Nodes.Count == 1)
							{
								if(fragment.Nodes[0] is XmlElement { ChildNodes.Count: > 0 })
									unTrim = false;
								else
									value = value.Trim();
							}
							else
							{
								unTrim = false;
							}
						}
					}
					catch(Exception exception)
					{
						this.Logger.LogErrorIfEnabled("Could not format comment correctly.", exception);
					}
				}

				if(unTrim)
					value = " " + value + (!string.IsNullOrEmpty(value) ? " " : string.Empty);

				comment.Value = value;
			}
		}

		protected internal virtual async Task FormatElements()
		{
			await Task.CompletedTask;
		}

		protected internal virtual async Task<string> GetLead(string value)
		{
			if(string.IsNullOrEmpty(value))
				return string.Empty;

			await Task.CompletedTask;

			const char newLineCharacter = '\n';

			var lead = value.Substring(0, value.Length - value.TrimStart().Length);

			lead = lead.Replace(Environment.NewLine, newLineCharacter.ToString());

			lead = lead.Split(newLineCharacter).Last();

			return lead;
		}

		#endregion
	}
}