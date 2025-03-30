namespace HansKindberg.Text.Formatting
{
	public interface IIndentableFormat
	{
		#region Properties

		bool Indent { get; }
		string IndentString { get; }
		string NewLineString { get; }

		#endregion
	}
}