using HansKindberg.Text.Formatting.Configuration;

namespace HansKindberg.Text.Formatting.Yaml.Configuration
{
	public class YamlFormatOptions
	{
		#region Properties

		public SortingOptions Sorting { get; set; } = new();

		#endregion
	}
}