namespace HansKindberg.Text.Formatting.Yaml.Models.Extensions
{
	public static class YamlNodeExtension
	{
		#region Methods

		//public static string GetSortValue(this YamlNode node)
		//{
		//	if(node == null)
		//		throw new ArgumentNullException(nameof(node));

		//	return node switch
		//	{
		//		YamlMappingNode mappingNode => mappingNode.Children.Keys
		//			.OfType<YamlScalarNode>()
		//			.Select(scalarNode => scalarNode.Value)
		//			.OrderBy(value => value)
		//			.FirstOrDefault() ?? string.Empty,

		//		YamlScalarNode scalarNode => scalarNode.Value ?? string.Empty,

		//		_ => string.Empty
		//	};
		//}

		//[SuppressMessage("Style", "IDE0010:Add missing cases")]
		//public static async Task Sort(this YamlNode node, IComparer<YamlNode> comparer)
		//{
		//	switch(node)
		//	{
		//		case YamlMappingNode mappingNode:
		//			{
		//				var sortedMappingNode = new YamlMappingNode();
		//				//foreach(var entry in mappingNode.Children.OrderBy(entry => ((YamlScalarNode)entry.Key).Value))
		//				foreach(var entry in mappingNode.Children.OrderBy(entry => entry.Key, comparer))
		//				{
		//					await entry.Key.Sort(comparer);
		//					await entry.Value.Sort(comparer);
		//					sortedMappingNode.Add(entry.Key, entry.Value);
		//				}

		//				mappingNode.Children.Clear();
		//				foreach(var entry in sortedMappingNode.Children)
		//				{
		//					mappingNode.Children.Add(entry.Key, entry.Value);
		//				}

		//				break;
		//			}
		//		case YamlSequenceNode sequenceNode:
		//			{
		//				if(sequenceNode.All(item => item is YamlScalarNode))
		//				{
		//					var sortedItems = sequenceNode.Cast<YamlScalarNode>().OrderBy(item => item.Value).ToList();
		//					sequenceNode.Children.Clear();
		//					foreach(var item in sortedItems)
		//					{
		//						sequenceNode.Add(item);
		//					}
		//				}
		//				else
		//				{
		//					foreach(var item in sequenceNode)
		//					{
		//						await item.Sort(comparer);
		//					}
		//				}

		//				break;
		//			}
		//	}
		//}

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