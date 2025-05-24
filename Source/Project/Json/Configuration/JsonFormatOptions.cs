using HansKindberg.Text.Formatting.Configuration;

namespace HansKindberg.Text.Formatting.Json.Configuration
{
	public class JsonFormatOptions
	{
		#region Properties

		public IndentationOptions Indentation { get; set; } = new();
		public AlphabeticalSortingOptions Sorting { get; set; } = new();

		#endregion
	}
}