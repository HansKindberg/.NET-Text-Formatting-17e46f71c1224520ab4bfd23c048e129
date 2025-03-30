using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Text.Formatting.Xml
{
	[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
	public interface IXmlAttribute
	{
		#region Properties

		int InitialIndex { get; }
		string? Name { get; }
		string? Value { get; }

		#endregion

		#region Methods

		bool Matches(string xPath);

		#endregion
	}
}