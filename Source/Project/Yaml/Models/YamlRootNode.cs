namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlRootNode : YamlNode
	{
		#region Properties

		public override int Level => 0;

		public override IYamlNode? Parent
		{
			get => null;
			set => throw new InvalidOperationException("Can not set parent for a root-node.");
		}

		#endregion
	}
}