using HansKindberg.Text.Formatting.Yaml.Configuration;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlSequenceItemNode : YamlNode, IYamlSequenceItemNode
	{
		#region Methods

		protected internal override IList<string> GetTextPartsExceptComment(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = new List<string>();

			if(!this.Flow)
				parts.Add(options.SequenceCharacter.ToString());

			return parts;
		}

		#endregion
	}
}