namespace HansKindberg.Text.Formatting.Yaml.Models.Extensions
{
	public static class YamlStreamExtension
	{
		#region Methods

		public static IEnumerable<IYamlNode> Descendants(this IYamlStream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));

			foreach(var document in stream.Documents)
			{
				foreach(var descendant in document.Descendants())
				{
					yield return descendant;
				}
			}
		}

		#endregion
	}
}