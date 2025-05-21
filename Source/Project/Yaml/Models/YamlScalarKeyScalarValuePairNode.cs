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
	}
}