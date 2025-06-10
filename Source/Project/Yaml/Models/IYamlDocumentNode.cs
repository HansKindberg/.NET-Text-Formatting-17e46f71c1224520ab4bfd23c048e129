using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDocumentNode : IYamlNode
	{
		#region Properties

		IList<IYamlDirective> Directives { get; }
		DocumentEnd End { get; }
		Comment? EndComment { get; set; }
		bool IncludeEndOnWrite { get; set; }
		bool IncludeStartOnWrite { get; set; }
		DocumentStart Start { get; }

		#endregion
	}
}