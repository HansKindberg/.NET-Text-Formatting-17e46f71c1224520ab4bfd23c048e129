using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class YamlFormatter(IParser<IList<IYamlNode>> parser, IParsingEventStringifier parsingEventStringifier) : ITextFormatter<YamlFormatOptions>
	{
		#region Properties

		protected internal virtual IParser<IList<IYamlNode>> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));
		protected internal virtual IParsingEventStringifier ParsingEventStringifier { get; } = parsingEventStringifier ?? throw new ArgumentNullException(nameof(parsingEventStringifier));

		#endregion

		#region Methods

		protected internal virtual async Task ApplyNamingConvention(IList<IYamlNode> nodes, YamlFormatOptions options)
		{
			if(nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(options.NamingConvention == null)
				return;

			var namingConvention = await this.GetNamingConvention(options.NamingConvention);

			if(namingConvention == null)
				return;

			foreach(var node in nodes)
			{
				foreach(var descendant in node.Descendants())
				{
					if(descendant == null) { }
					//if(descendant.Key is Scalar { IsKey: true } scalar)
					//	descendant.Key = new Scalar(scalar.Anchor, scalar.Tag, namingConvention.Apply(scalar.Value), scalar.Style, scalar.IsPlainImplicit, scalar.IsQuotedImplicit, scalar.Start, scalar.End, scalar.IsKey);
				}
			}
		}

		protected internal virtual async Task<IComparer<IYamlNode>> CreateYamlNodeComparer(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			return new YamlNodeComparer(options);
		}

		public virtual async Task<string> Format(YamlFormatOptions options, string text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var nodes = await this.Parser.Parse(text);

			if(nodes.Count == 0)
				return string.Empty;

			await this.ApplyNamingConvention(nodes, options);

			await this.Sort(nodes, options);

			var value = await this.GetText(nodes, options);

			return value;
		}

		protected internal virtual async Task<INamingConvention?> GetNamingConvention(NamingConvention? namingConvention)
		{
			await Task.CompletedTask;

			if(namingConvention == null)
				return null;

			return namingConvention switch
			{
				NamingConvention.CamelCase => CamelCaseNamingConvention.Instance,
				NamingConvention.Hyphenated => HyphenatedNamingConvention.Instance,
				NamingConvention.LowerCase => LowerCaseNamingConvention.Instance,
				NamingConvention.PascalCase => PascalCaseNamingConvention.Instance,
				NamingConvention.Underscored => UnderscoredNamingConvention.Instance,
				_ => null
			};
		}

		protected internal virtual async Task<string> GetText(IList<IYamlNode> nodes, YamlFormatOptions options)
		{
			if(nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var lines = new List<string>();
			var multipleDocuments = nodes.Count > 1;

			foreach(var node in nodes)
			{
				if(multipleDocuments && !string.IsNullOrEmpty(options.StartOfDocument))
					lines.Add(options.StartOfDocument);

				await node.Write(lines, options, this.ParsingEventStringifier);

				if(multipleDocuments && !string.IsNullOrEmpty(options.EndOfDocument))
					lines.Add(options.EndOfDocument!);
			}

			return string.Join(options.NewLine, lines).Trim();
		}

		protected internal virtual async Task Sort(IList<IYamlNode> nodes, YamlFormatOptions options)
		{
			if(nodes == null)
				throw new ArgumentNullException(nameof(nodes));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(!options.DocumentSorting.Enabled && !options.ScalarSorting.Enabled && !options.SequenceSorting.Enabled)
				return;

			var comparer = await this.CreateYamlNodeComparer(options);

			foreach(var node in nodes)
			{
				await node.Sort(comparer);
			}

			if(options.DocumentSorting.Enabled)
			{
				nodes.Sort(comparer);
			}
		}

		#endregion
	}
}