using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlFlowSequenceNode(FlowSequenceStart token) : YamlStructureNode
	{
		#region Properties

		public virtual FlowSequenceStart Token { get; } = token ?? throw new ArgumentNullException(nameof(token));

		#endregion
	}
}