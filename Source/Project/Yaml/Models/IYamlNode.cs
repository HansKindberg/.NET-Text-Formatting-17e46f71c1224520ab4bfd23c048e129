namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlNode : IFormatableYaml, IYamlCommentSurroundable
	{
		#region Properties

		IEnumerable<IYamlNode> Children { get; }
		bool Flow { get; set; }

		/// <summary>
		/// The structural depth of the node in the logical tree.
		/// </summary>
		int Level { get; }

		IYamlNode? Parent { get; set; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);

		#endregion
	}
}