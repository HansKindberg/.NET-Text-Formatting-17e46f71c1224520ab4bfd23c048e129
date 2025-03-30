using System.ComponentModel;

namespace HansKindberg.Text.Formatting.Configuration
{
	public class SortingOptions
	{
		#region Properties

		public ListSortDirection Direction { get; set; } = ListSortDirection.Ascending;
		public bool Enabled { get; set; } = true;

		#endregion
	}
}