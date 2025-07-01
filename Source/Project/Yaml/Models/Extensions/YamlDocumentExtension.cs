namespace HansKindberg.Text.Formatting.Yaml.Models.Extensions
{
	public static class YamlDocumentExtension
	{
		#region Methods

		public static IEnumerable<IYamlNode> Descendants(this IYamlDocument document)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));

			foreach(var node in document.Nodes)
			{
				yield return node;

				foreach(var descendant in node.Descendants())
				{
					yield return descendant;
				}
			}
		}

		#endregion
	}
}