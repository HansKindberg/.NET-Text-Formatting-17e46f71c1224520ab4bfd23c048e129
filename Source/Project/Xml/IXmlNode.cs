namespace HansKindberg.Text.Formatting.Xml
{
	public interface IXmlNode
	{
		#region Properties

		int InitialIndex { get; }
		IXmlNode? InitialNextNode { get; }
		IXmlNode? InitialPreviousNode { get; }
		string Name { get; }
		string Value { get; }

		#endregion

		#region Methods

		bool Matches(string xPath);

		#endregion
	}
}