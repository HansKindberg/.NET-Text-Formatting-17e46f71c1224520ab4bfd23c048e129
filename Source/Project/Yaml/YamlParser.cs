using HansKindberg.Text.Formatting.Extensions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using IYamlDotNetParser = YamlDotNet.Core.IParser;

namespace HansKindberg.Text.Formatting.Yaml
{
	public class YamlParser : IParser<IList<YamlNode>>
	{
		#region Fields

		private const int _informationalYamlMaximumLength = 100;

		#endregion

		#region Methods

		protected internal virtual async Task<IScanner> CreateScanner(TextReader textReader)
		{
			await Task.CompletedTask;

			return new Scanner(textReader, true); // Comments not supported when deserializing.
		}

		protected internal virtual async Task<IYamlDotNetParser> CreateYamlDotNetParser(TextReader textReader)
		{
			return new Parser(await this.CreateScanner(textReader));
		}

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value is { Length: > _informationalYamlMaximumLength })
				value = $"{value.Substring(0, _informationalYamlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<IList<YamlNode>> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				await Task.CompletedTask;

				var yamlStream = new YamlStream();

				using(var stringReader = new StringReader(value))
				{
					yamlStream.Load(await this.CreateYamlDotNetParser(stringReader));
				}

				var yamlNodes = yamlStream.Documents.Select(document => document.RootNode).ToList();

				return yamlNodes;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-document.", exception);
			}
		}

		#endregion
	}
}