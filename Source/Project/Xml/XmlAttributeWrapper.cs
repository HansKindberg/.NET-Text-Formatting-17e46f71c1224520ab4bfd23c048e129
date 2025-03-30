using System.Xml;
using Microsoft.Extensions.Logging;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc cref="IXmlAttribute" />
	public class XmlAttributeWrapper(ILogger? logger, XmlAttribute xmlAttribute) : XmlNodeWrapper<XmlAttribute>(logger, xmlAttribute), IXmlAttribute
	{
		#region Properties

		public override string Value => this.XmlNode.InnerXml;

		#endregion
	}
}