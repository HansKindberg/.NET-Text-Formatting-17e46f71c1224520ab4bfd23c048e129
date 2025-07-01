namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlStructureNode : YamlNode, IYamlStructureNode
	{
		#region Properties

		/// <summary>
		/// Same as parent. We do not step up the indentation level for structural nodes.
		/// </summary>
		public override int IndentationLevel => this.Parent?.IndentationLevel ?? 0;

		#endregion
	}
}