using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public interface IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> Children { get; }
		int Level { get; }
		IYamlNode? Parent { get; set; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);
		Task ApplyNamingConvention(INamingConvention namingConvention);
		Task Sort(IComparer<IYamlNode> comparer);
		Task Write(IList<string> lines, YamlFormatOptions options);

		#endregion
	}
}