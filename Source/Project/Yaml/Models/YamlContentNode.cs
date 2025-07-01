using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlContentNode : YamlNode, IYamlContentNode
	{
		#region Fields

		private Comment? _comment;
		private Scalar? _key;
		private Scalar? _value;

		#endregion

		#region Properties

		public virtual Anchor? Anchor { get; set; }
		public virtual AnchorAlias? AnchorAlias { get; set; }

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
	}
}