using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.XPath;
using HansKindberg.Text.Formatting.Extensions;
using HansKindberg.Text.Formatting.Logging.Extensions;
using Microsoft.Extensions.Logging;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc cref="IXmlNode" />
	public class XmlNodeWrapper(ILogger logger, XmlNode? xmlNode) : XmlNodeWrapper<XmlNode>(logger, xmlNode), IXmlNode
	{
		#region Properties

		public virtual IXmlNode? InitialNextNode { get; set; }
		public virtual IXmlNode? InitialPreviousNode { get; set; }

		public virtual int Level
		{
			get
			{
				var level = 0;

				var parent = this.XmlNode.ParentNode;

				while(parent != null)
				{
					level++;

					parent = parent.ParentNode;
				}

				return level;
			}
		}

		#endregion
	}

	public class XmlNodeWrapper<T>(ILogger? logger, T? xmlNode) where T : XmlNode
	{
		#region Properties

		public virtual int InitialIndex { get; set; }
		protected internal virtual ILogger? Logger { get; } = logger;
		public virtual string Name => this.XmlNode.Name;
		public virtual string Value => this.XmlNode.Value;
		public virtual T XmlNode { get; } = xmlNode ?? throw new ArgumentNullException(nameof(xmlNode));

		#endregion

		#region Methods

		protected internal virtual XPathExpression CreateXPathExpression(IXmlNamespaceResolver xmlNamespaceResolver, string xPath)
		{
			try
			{
				return XPathExpression.Compile(xPath, xmlNamespaceResolver);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not create a XPath-expression from XPath {xPath.ToStringRepresentation()}.", exception);
			}
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public virtual bool Matches(string xPath)
		{
			try
			{
				var navigator = this.XmlNode.CreateNavigator();

				return navigator.Matches(this.CreateXPathExpression(navigator, xPath));
			}
			catch(Exception exception)
			{
				this.Logger?.LogErrorIfEnabled(exception, $"Could not match {this.GetType().Name} with XPath {xPath.ToStringRepresentation()}.");

				return false;
			}
		}

		#endregion
	}
}