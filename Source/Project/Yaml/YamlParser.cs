using HansKindberg.Text.Formatting.Extensions;
using YamlDotNet.RepresentationModel;

namespace HansKindberg.Text.Formatting.Yaml
{
	// TODO: Look over this class.
	public class YamlParser : IParser<YamlDocument>
	{
		#region Fields

		private const int _informationalYamlMaximumLength = 100;

		#endregion

		#region Methods

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value is { Length: > _informationalYamlMaximumLength })
				value = $"{value.Substring(0, _informationalYamlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<YamlDocument> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				await Task.CompletedTask;

				//var yamlStream = new YamlStream();
				//yamlStream.Load(new StringReader(yaml));

				//// Retrieve the first (or only) document in the stream
				//YamlDocument document = yamlStream.Documents[0];

				//// Get the root node of the document (usually a mapping node)
				//YamlMappingNode rootNode = (YamlMappingNode)document.RootNode;

				return new YamlDocument(value);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-document.", exception);
			}
		}

		#endregion
	}
}