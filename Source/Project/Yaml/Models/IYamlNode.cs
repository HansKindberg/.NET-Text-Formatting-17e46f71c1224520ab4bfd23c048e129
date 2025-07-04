namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlNode : IYamlComponent, IFormatableYaml, IYamlCommentSurroundable
	{
		#region Properties

		IEnumerable<IYamlNode> Children { get; }
		bool Flow { get; set; }

		/// <summary>
		/// To be able to use stable sorting.
		/// </summary>
		int Index { get; set; }

		/// <summary>
		/// The structural depth of the node in the logical tree.
		/// </summary>
		int Level { get; }

		IYamlNode? Parent { get; set; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);
		Task Sort(IComparer<IYamlNode> comparer);

		#endregion
	}
}