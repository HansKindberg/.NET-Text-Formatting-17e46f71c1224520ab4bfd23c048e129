using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> Children { get; }
		Comment? Comment { get; set; }
		IList<Comment> LeadingComments { get; }
		int Level { get; }
		IYamlNode? Parent { get; set; }
		bool Sequence { get; set; }
		IList<Comment> TrailingComments { get; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);
		Task ApplyNamingConvention(INamingConvention namingConvention);
		Task Sort(IComparer<IYamlNode> comparer);
		string ToString(YamlFormatOptions options);
		Task Write(IList<string> lines, YamlFormatOptions options);

		#endregion
	}
}