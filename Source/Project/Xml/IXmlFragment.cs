using System.Xml;

namespace HansKindberg.Text.Formatting.Xml
{
	public interface IXmlFragment
	{
		#region Properties

		IList<XmlNode> Nodes { get; }

		#endregion
	}
}