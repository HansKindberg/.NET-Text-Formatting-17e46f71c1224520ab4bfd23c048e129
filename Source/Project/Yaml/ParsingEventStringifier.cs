using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml
{
	/// <inheritdoc />
	public class ParsingEventStringifier : IParsingEventStringifier
	{
		#region Methods

		protected internal virtual async Task<string> DoubleQuoted(string value)
		{
			await Task.CompletedTask;

			return $"\"{value}\"";
		}

		protected internal virtual async Task<string?> GetValue(Quotation? quotation, Scalar scalar)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			var value = scalar.Value;

			return quotation switch
			{
				null when !scalar.IsQuotedImplicit => value,
#pragma warning disable IDE0072 // Add missing cases
				null => scalar.Style switch
				{
					ScalarStyle.DoubleQuoted => await this.DoubleQuoted(value),
					ScalarStyle.SingleQuoted => await this.SingleQuoted(value),
					_ => value
				},
#pragma warning restore IDE0072 // Add missing cases
				Quotation.Clear => value,
				Quotation.Double => await this.DoubleQuoted(value),
				Quotation.Single => await this.SingleQuoted(value),
				_ => value
			};
		}

		protected internal virtual async Task<string> SingleQuoted(string value)
		{
			await Task.CompletedTask;

			return $"'{value}'";
		}

		public virtual async Task<string?> Stringify(YamlFormatOptions options, ParsingEvent? parsingEvent)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			return parsingEvent switch
			{
				AnchorAlias anchorAlias => await this.StringifyAnchorAlias(anchorAlias, options),
				Scalar scalar => await this.StringifyScalar(options, scalar),
				_ => null
			};
		}

		protected internal virtual async Task<string?> StringifyAnchorAlias(AnchorAlias anchorAlias, YamlFormatOptions options)
		{
			if(anchorAlias == null)
				throw new ArgumentNullException(nameof(anchorAlias));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			await Task.CompletedTask;

			return anchorAlias.Value != null && anchorAlias.Value.IsEmpty ? null : $"{options.AnchorAliasPrefix}{anchorAlias.Value.Value}";
		}

		protected internal virtual async Task<string?> StringifyScalar(YamlFormatOptions options, Scalar scalar)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			var value = await this.GetValue(scalar.IsKey ? options.KeyQuotation : options.ValueQuotation, scalar);

			if(scalar.IsKey)
				return $"{value}{options.KeySuffix}";

			string? anchor = null;
			if(scalar.Anchor != null && !scalar.Anchor.IsEmpty)
				anchor = scalar.Anchor.Value;

			string? tag = null;
			if(scalar.Tag != null! && !scalar.Tag.IsEmpty)
				tag = scalar.Tag.Value;

			var parts = new List<string>();

			if(anchor != null)
				parts.Add($"{options.AnchorPrefix}{anchor}");

			if(tag != null)
				parts.Add($"{options.TagPrefix}{tag}");

			if(value != null)
				parts.Add(value);

			if(parts.Count < 1)
				return null;

			return string.Join(options.Space.ToString(), parts);
		}

		#endregion
	}
}