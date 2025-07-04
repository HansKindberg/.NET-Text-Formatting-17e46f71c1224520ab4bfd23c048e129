using HansKindberg.Text.Formatting.Yaml.Configuration;
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

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = new List<string>();

			//	var key = this.GetKey(options.KeyQuotation);
			//	if(!string.IsNullOrWhiteSpace(key))
			//		parts.Add($"{key}{options.KeySuffix}");

			//	var tag = this.GetTag();
			//	if(!string.IsNullOrWhiteSpace(tag))
			//		parts.Add($"{options.TagPrefix}{tag}");

			//	var anchor = this.GetAnchor();
			//	if(!string.IsNullOrWhiteSpace(anchor))
			//		parts.Add($"{options.AnchorPrefix}{anchor}");

			//	var anchorAlias = this.GetAnchorAlias();
			//	if(!string.IsNullOrWhiteSpace(anchorAlias))
			//		parts.Add($"{options.AnchorAliasPrefix}{anchorAlias}");

			//	var value = this.GetValue(options.ValueQuotation);
			//	if(!string.IsNullOrWhiteSpace(value))
			//		parts.Add(value!);

			//	var comment = this.GetComment();
			//	if(comment != null)
			//		parts.Add($"{options.CommentPrefix}{comment}");

			return parts;
		}

		#endregion
	}
}