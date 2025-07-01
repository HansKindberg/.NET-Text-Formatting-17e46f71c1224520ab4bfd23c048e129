using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlScalarKeyNode : YamlNode
	{
		#region Constructors

		public YamlScalarKeyNode(Scalar key)
		{
			if(key == null)
				throw new ArgumentNullException(nameof(key));

			if(!key.IsKey)
				throw new ArgumentException("The key-scalar must be a key.", nameof(key));

			this.Key = key;
		}

		#endregion

		#region Properties

		public virtual Scalar Key { get; private set; }

		#endregion

		#region Methods

		public override async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			if(namingConvention == null)
				throw new ArgumentNullException(nameof(namingConvention));

			this.Key = new Scalar(namingConvention.Apply(this.Key.Value), this.Key.Style, this.Key.Start, this.Key.End)
			{
				IsKey = this.Key.IsKey
			};

			await base.ApplyNamingConvention(namingConvention);
		}

		protected internal override string? GetKey(Quotation? quotation)
		{
			return this.GetText(quotation, this.Key);
		}

		#endregion
	}
}