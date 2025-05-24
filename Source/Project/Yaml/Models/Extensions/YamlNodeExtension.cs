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

		public static bool IsFirstChild(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return false;

			if(!node.Parent.Children.Any())
				return false;

			var firstChild = node.Parent.Children.FirstOrDefault();

			return ReferenceEquals(firstChild, node);
		}

		public static bool IsLastChild(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return false;

			if(!node.Parent.Children.Any())
				return false;

			var lastChild = node.Parent.Children.LastOrDefault();

			return ReferenceEquals(lastChild, node);
		}

		#endregion
	}
}