using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlComponent" />
	public abstract class YamlComponent : YamlObject, IYamlComponent
	{
		#region Properties

		public virtual Comment? Comment { get; set; }

		#endregion

		#region Methods

		protected internal virtual string? GetComment()
		{
			return this.GetCommentValue(this.Comment);
		}

		protected internal virtual IList<string> GetTextParts(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = this.GetTextPartsExceptComment(options);

			if(this.Comment != null)
				parts.Add($"{options.CommentPrefix}{this.GetComment()}");

			return parts;
		}

		protected internal abstract IList<string> GetTextPartsExceptComment(YamlFormatOptions options);

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