using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public abstract class YamlScalarKeyValuePairNode<TValue> : YamlKeyValuePairNode<Scalar, TValue> where TValue : Token
	{
		#region Constructors

		protected YamlScalarKeyValuePairNode(Scalar key, TValue value) : base(key, value)
		{
			if(!key.IsKey)
				throw new ArgumentException("The key-scalar must be a key.", nameof(key));
		}

		#endregion
	}
}