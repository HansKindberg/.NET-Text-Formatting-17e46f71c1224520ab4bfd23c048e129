using HansKindberg.Text.Formatting.Extensions;
using HansKindberg.Text.Formatting.Yaml.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class NewYamlParser : IParser<IYamlNode>
	{
		#region Properties

		protected internal virtual int InformationalYamlMaximumLength => 100;

		#endregion

		#region Methods

		protected internal virtual async Task<IYamlNode> CreateNode(IList<Token> tokens)
		{
			if(tokens == null)
				throw new ArgumentNullException(nameof(tokens));

			var comments = new List<Comment>();
			var root = await this.CreateNodeForStream(parsingEvents);
			var parent = root;

			while(tokens.Count > 0)
			{

			}

			return new YamlStreamNode(new StreamStart(), new StreamEnd());
		}

		protected internal virtual async Task<IScanner> CreateScanner(TextReader textReader)
		{
			await Task.CompletedTask;

			return new Scanner(textReader, false);
		}

		protected internal virtual async Task<IList<Token>> CreateTokens(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			var tokens = new List<Token>();

			using(var stringReader = new StringReader(value))
			{
				var scanner = await this.CreateScanner(stringReader);

				while(scanner.MoveNext())
				{
					tokens.Add(scanner.Current!);
				}
			}

			return tokens;
		}

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value != null && value.Length > this.InformationalYamlMaximumLength)
				value = $"{value.Substring(0, this.InformationalYamlMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<IYamlNode> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				var tokens = await this.CreateTokens(value);

				var node = await this.CreateNode(tokens);

				return node;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to yaml-node.", exception);
			}
		}

		#endregion
	}
}