using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	public class YamlScalarValueNode : YamlValueNode<Scalar>
	{
		#region Constructors

		public YamlScalarValueNode(Scalar value) : base(value)
		{
			if(value.IsKey)
				throw new ArgumentException("The value-scalar can not be a key.", nameof(value));
		}

		#endregion
	}
}