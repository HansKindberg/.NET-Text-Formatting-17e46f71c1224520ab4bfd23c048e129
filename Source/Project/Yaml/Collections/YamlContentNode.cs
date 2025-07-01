using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlContentNode" />
	public abstract class YamlContentNode : YamlNode, IYamlContentNode, IYamlComponent
	{
		#region Properties

		public virtual Comment? Comment { get; set; }
		public virtual IList<Comment> LeadingComments { get; } = [];
		//public virtual bool Sequence { get; set; }
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion

		#region Methods

		public virtual string ToString(YamlFormatOptions options)
		{
			throw new NotImplementedException("adfkadfkjadfkadjfalkdfjalkdfjakldfjkladfjkaldsfjkadfjkadfjkadsjfkladfjkladjfadklfj");
		}

		#endregion
	}
}