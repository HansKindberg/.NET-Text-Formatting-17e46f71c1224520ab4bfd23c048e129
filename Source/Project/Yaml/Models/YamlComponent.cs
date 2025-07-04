using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlComponent : YamlObject, IYamlComponent
	{
		#region Fields

		private Comment? _comment;
		private static readonly YamlFormatOptions _defaultYamlFormatOptions = new();

		#endregion

		#region Properties

		public virtual Comment? Comment
		{
			get => this._comment;
			set
			{
				if(value is { IsInline: false })
					throw new ArgumentException("The comment must be inline.", nameof(value));

				this._comment = value;
			}
		}

		#endregion

		#region Methods

		protected internal virtual IList<string> GetTextParts(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = this.GetTextPartsExceptComment(options);

			if(this.Comment != null)
				parts.Add($"{options.CommentPrefix}{this.GetCommentValue(this.Comment, options)}");

			return parts;
		}

		protected internal abstract IList<string> GetTextPartsExceptComment(YamlFormatOptions options);

		public override string ToString()
		{
			return this.ToString(_defaultYamlFormatOptions);
		}

		public virtual string ToString(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = this.GetTextParts(options);

			return parts.Any() ? string.Join(options.Space.ToString(), parts) : string.Empty;
		}

		#endregion
	}
}