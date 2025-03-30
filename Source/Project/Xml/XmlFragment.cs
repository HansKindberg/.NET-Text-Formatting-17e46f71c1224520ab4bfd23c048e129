using System.Xml;

namespace HansKindberg.Text.Formatting.Xml
{
	public class XmlFragment : IXmlFragment
	{
		#region Properties

		public virtual IList<XmlNode> Nodes { get; } = [];

		#endregion
	}
}