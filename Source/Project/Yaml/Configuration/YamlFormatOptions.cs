using HansKindberg.Text.Formatting.Configuration;
using HansKindberg.Text.Formatting.Yaml.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Configuration
{
	public class YamlFormatOptions
	{
		#region Properties

		public NamingConvention? NamingConvention { get; set; }
		public SortingOptions Sorting { get; set; } = new();

		#endregion
	}
}