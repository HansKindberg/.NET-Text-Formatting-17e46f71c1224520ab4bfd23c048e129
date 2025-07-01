using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlFlowMappingNode(FlowMappingStart token) : YamlStructureNode
	{
		#region Properties

		public virtual FlowMappingStart Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}