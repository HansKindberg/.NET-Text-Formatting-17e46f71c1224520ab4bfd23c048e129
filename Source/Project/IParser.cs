namespace HansKindberg.Text.Formatting
{
	public interface IParser<T>
	{
		#region Methods

		Task<T> Parse(string value);

		#endregion
	}
}