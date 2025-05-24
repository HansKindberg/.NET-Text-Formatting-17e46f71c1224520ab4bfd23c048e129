using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
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

		protected internal override async Task<string?> GetAnchor()
		{
			await Task.CompletedTask;

			return this.Value.Anchor.IsEmpty ? null : this.Value.Anchor.Value;
		}

		protected internal override async Task<string?> GetTag()
		{
			await Task.CompletedTask;

			return this.Value.Tag.IsEmpty ? null : this.Value.Tag.Value;
		}

		protected internal override async Task<string?> GetValue(Quotation? quotation)
		{
			return await this.GetText(quotation, this.Value);
		}

		#endregion
	}
}