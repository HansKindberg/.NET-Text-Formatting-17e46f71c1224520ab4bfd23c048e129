using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public abstract class YamlKeyValuePairNode<TKey, TValue>(TKey key, TValue value) : YamlNode where TKey : Token where TValue : Token
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
				this.Key = (TKey)(Token)new Scalar(namingConvention.Apply(key.Value), key.Style, key.Start, key.End)
				{
					IsKey = key.IsKey
				};

			await base.ApplyNamingConvention(namingConvention);
		}

		protected internal override string? GetKey(Quotation? quotation)
		{
			if(this.Key is Scalar key)
				return this.GetText(quotation, key);

			return null;
		}

		#endregion
	}
}