using HansKindberg.Text.Formatting.Collections.Generic.Extensions;

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

		public static bool IsFirstSibling(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return false;

			if(!node.Parent.Children.Any())
				return false;

			var firstSibling = node.Parent.Children.FirstOrDefault();

			return ReferenceEquals(firstSibling, node);
		}

		public static bool IsLastSibling(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return false;

			if(!node.Parent.Children.Any())
				return false;

			var lastSibling = node.Parent.Children.LastOrDefault();

			return ReferenceEquals(lastSibling, node);
		}

		public static IYamlNode? NextSibling(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return null;

			if(!node.Parent.Children.Any())
				return null;

			var index = node.Parent.Children.IndexOf(node);

			if(index < 0 || index >= node.Parent.Children.Count() - 1)
				return null;

			return node.Parent.Children.ElementAt(index + 1);
		}

		public static IYamlNode? PreviousSibling(this IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			if(node.Parent == null)
				return null;

			if(!node.Parent.Children.Any())
				return null;

			var index = node.Parent.Children.IndexOf(node);

			return index < 1 ? null : node.Parent.Children.ElementAt(index - 1);
		}

		#endregion
	}
}