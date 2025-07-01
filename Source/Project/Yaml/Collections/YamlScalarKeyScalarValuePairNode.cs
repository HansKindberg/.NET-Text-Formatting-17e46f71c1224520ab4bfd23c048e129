using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlScalarKeyScalarValuePairNode : YamlScalarKeyValuePairNode<Scalar>
	{
		#region Constructors

		public YamlScalarKeyScalarValuePairNode(Scalar key, Scalar value) : base(key, value)
		{
			if(value.IsKey)
				throw new ArgumentException("The value-scalar can not be a key.", nameof(value));
		}

		#endregion

		#region Methods

		protected internal override string? GetAnchor()
		{
			return null;
			//return this.Value.Anchor.IsEmpty ? null : this.Value.Anchor.Value;
		}

		protected internal override string? GetTag()
		{
			return null;
			//return this.Value.Tag.IsEmpty ? null : this.Value.Tag.Value;
		}

		protected internal override string? GetValue(Quotation? quotation)
		{
			return this.GetText(quotation, this.Value);
		}

		#endregion
	}
}