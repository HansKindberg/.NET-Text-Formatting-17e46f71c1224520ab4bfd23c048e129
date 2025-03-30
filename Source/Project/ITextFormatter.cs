namespace HansKindberg.Text.Formatting
{
	public interface ITextFormatter<in TOptions>
	{
		#region Methods

		Task<string> Format(TOptions options, string text);

		#endregion
	}
}