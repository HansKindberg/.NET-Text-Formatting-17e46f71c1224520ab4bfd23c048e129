namespace HansKindberg.Text.Formatting.Configuration
{
	public class AlphabeticalSortingOptions : SortingOptions
	{
		#region Properties

		public StringComparison Comparison { get; set; } = StringComparison.OrdinalIgnoreCase;

		#endregion
	}
}