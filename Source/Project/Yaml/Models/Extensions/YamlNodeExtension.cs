namespace HansKindberg.Text.Formatting.Yaml.Models.Extensions
{
	public static class YamlNodeExtension
	{
		#region Methods

		public static IEnumerable<IYamlNode> Descendants(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			foreach(var child in node.Children)
			{
				yield return child;

				foreach(var descendant in child.Descendants())
				{
					yield return descendant;
				}
			}
		}

		#endregion
	}
}