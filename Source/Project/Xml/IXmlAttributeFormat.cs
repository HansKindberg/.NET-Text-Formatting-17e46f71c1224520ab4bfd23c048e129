using System.ComponentModel;

namespace HansKindberg.Text.Formatting.Xml
{
	public interface IXmlAttributeFormat
	{
		#region Properties

		ListSortDirection AlphabeticalSortDirection { get; }
		StringComparison NameComparison { get; }

		/// <summary>
		/// XPath-expressions for pinned attributes.
		/// </summary>
		IEnumerable<string> Pinned { get; }

		bool SortAlphabetically { get; }
		StringComparison ValueComparison { get; }

		#endregion
	}
}