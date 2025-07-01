namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlNode : IFormatableYaml, IYamlCommentSurroundable
	{
		#region Properties

		//////////////////////////////////////////////Anchor? Anchor { get; }
		//////////////////////////////////////////////AnchorAlias? AnchorAlias { get; }
		//////////////////////////////////////////////BlockEntry? BlockEntry { get; }
		IEnumerable<IYamlNode> Children { get; }
		////////////////////////////////Comment? Comment { get; }
		////////////////////////////////Scalar? Key { get; }


		/// <summary>
		/// The structural depth of the node in the logical tree.
		/// </summary>
		int Level { get; }

		/// <summary>
		/// The number of indentation steps for YAML formatting.
		/// This may differ from <see cref="Level" /> depending on formatting needs.
		/// </summary>
		int IndentationLevel { get; }













		IYamlNode? Parent { get; set; }
		////////////////////////////////Tag? Tag { get; }
		////////////////////////////////Scalar? Value { get; }

		#endregion

		#region Methods

		Task Add(IYamlNode child);

		#endregion
	}
}