using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlFlowEntryNode(FlowEntry token) : YamlStructureNode, IYamlFlowEntryNode
	{
		#region Properties

		public virtual FlowEntry Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}