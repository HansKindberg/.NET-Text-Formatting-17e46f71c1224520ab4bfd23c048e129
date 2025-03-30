using System.ComponentModel;

namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc />
	public class XmlAttributeFormat : IXmlAttributeFormat
	{
		#region Properties

		public virtual ListSortDirection AlphabeticalSortDirection { get; set; } = ListSortDirection.Ascending;
		public virtual StringComparison NameComparison { get; set; } = StringComparison.Ordinal;
		IEnumerable<string> IXmlAttributeFormat.Pinned => this.Pinned;
		public virtual IList<string> Pinned { get; } = [];
		public virtual bool SortAlphabetically { get; set; } = true;
		public virtual StringComparison ValueComparison { get; set; } = StringComparison.Ordinal;

		#endregion
	}
}