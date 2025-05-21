using HansKindberg.Text.Formatting.Diagnostics;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlScalarKeyNode : YamlNode
	{
		#region Fields

		private TextInformation? _textInformation;

		#endregion

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

		public virtual Scalar Key { get; }

		public override TextInformation TextInformation
		{
			get
			{
				this._textInformation ??= this.CreateTextInformation(this.Key, this.Key);

				return this._textInformation.Value;
			}
		}

		#endregion
	}
}