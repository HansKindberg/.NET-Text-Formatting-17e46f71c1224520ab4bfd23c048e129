using HansKindberg.Text.Formatting.Diagnostics;
using HansKindberg.Text.Formatting.Yaml.Configuration;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> Children { get; }
		string? Comment { get; set; }
		IList<string> LeadingComments { get; }
		int Level { get; }
		IYamlNode? Parent { get; set; }
		bool Sequence { get; set; }
		TextInformation TextInformation { get; }
		IList<string> TrailingComments { get; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);
		Task Sort(IComparer<IYamlNode> comparer);
		Task Write(IList<string> lines, YamlFormatOptions options, IParsingEventStringifier parsingEventStringifier);

		#endregion
	}
}