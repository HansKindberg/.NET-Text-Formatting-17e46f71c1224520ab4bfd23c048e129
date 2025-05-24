using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlKeyValuePairNode<TKey, TValue>(TKey key, TValue value) : YamlNode where TKey : ParsingEvent where TValue : ParsingEvent
	{
		#region Properties

		public virtual TKey Key { get; private set; } = key ?? throw new ArgumentNullException(nameof(key));
		public virtual TValue Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

		#endregion

		#region Methods

		public override async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			if(namingConvention == null)
				throw new ArgumentNullException(nameof(namingConvention));

			if(this.Key is Scalar key)
				this.Key = (TKey)(ParsingEvent)new Scalar(key.Anchor, key.Tag, namingConvention.Apply(key.Value), key.Style, key.IsPlainImplicit, key.IsQuotedImplicit, key.Start, key.End, key.IsKey);

			await base.ApplyNamingConvention(namingConvention);
		}

		protected internal override async Task<string?> GetKey(Quotation? quotation)
		{
			if(this.Key is Scalar key)
				return await this.GetText(quotation, key);

			return null;
		}

		#endregion
	}
}