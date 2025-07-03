using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IFormatableYaml
	{
		#region Methods

		Task ApplyNamingConvention(INamingConvention namingConvention);
		Task Sort(IComparer<IYamlNode> comparer);
		Task Write(IList<string> lines, YamlFormatOptions options);

		#endregion
	}
}