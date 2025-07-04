using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlContentNode : YamlNode, IYamlContentNode
	{
		#region Fields

		private Scalar? _key;
		private Scalar? _value;

		#endregion

		#region Properties

		public virtual Anchor? Anchor { get; set; }
		public virtual AnchorAlias? AnchorAlias { get; set; }

		public virtual Scalar? Key
		{
			get => this._key;
			set
			{
				if(value is { IsKey: false })
					throw new ArgumentException("The scalar must be a key.", nameof(value));

				this._key = value;
			}
		}

		public virtual Tag? Tag { get; set; }

		public virtual Scalar? Value
		{
			get => this._value;
			set
			{
				if(value is { IsKey: true })
					throw new ArgumentException("The scalar must not be a key.", nameof(value));

				this._value = value;
			}
		}

		#endregion

		#region Methods

		protected internal virtual string DoubleQuoted(string text)
		{
			return $"\"{text}\"";
		}

		protected internal virtual string GetScalarText(Quotation? quotation, Scalar scalar)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			var text = scalar.Value;

			if(quotation != null)
				return text;

			return quotation switch
			{
#pragma warning disable IDE0072 // Add missing cases
				null => scalar.Style switch
				{
					ScalarStyle.DoubleQuoted => this.DoubleQuoted(text),
					ScalarStyle.SingleQuoted => this.SingleQuoted(text),
					_ => text
				},
#pragma warning restore IDE0072 // Add missing cases
				Quotation.Clear => text,
				Quotation.Double => this.DoubleQuoted(text),
				Quotation.Single => this.SingleQuoted(text),
				_ => text
			};
		}

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = new List<string>();

			if(this.Key != null)
			{
				var key = this.GetScalarText(options.KeyQuotation, this.Key);
				parts.Add($"{key}{options.KeySuffix}");
			}

			if(this.Tag != null)
			{
				var tag = $"{this.Tag.Handle}{options.TagDelimiter}{this.Tag.Suffix}";
				parts.Add($"{options.TagPrefix}{tag}");
			}

			if(this.Anchor != null)
			{
				var anchor = this.Anchor.Value;
				parts.Add($"{options.AnchorPrefix}{anchor}");
			}

			if(this.AnchorAlias != null)
			{
				var anchorAlias = this.AnchorAlias.Value;
				parts.Add($"{options.AnchorAliasPrefix}{anchorAlias}");
			}

			if(this.Value != null)
			{
				var value = this.GetScalarText(options.ValueQuotation, this.Value);
				parts.Add(value);
			}

			return parts;
		}

		protected internal virtual string SingleQuoted(string text)
		{
			return $"'{text}'";
		}

		#endregion
	}
}