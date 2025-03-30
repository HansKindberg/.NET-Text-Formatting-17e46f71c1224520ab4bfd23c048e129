namespace HansKindberg.Text.Formatting.Xml
{
	public interface IFormattableXml
	{
		#region Methods

		Task<string> Format();

		#endregion
	}
}