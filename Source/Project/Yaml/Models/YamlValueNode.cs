using HansKindberg.Text.Formatting.Diagnostics;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlValueNode<T>(T value) : YamlNode where T : ParsingEvent
	{
		#region Fields

		private TextInformation? _textInformation;

		#endregion

		#region Properties

		public override TextInformation TextInformation
		{
			get
			{
				this._textInformation ??= this.CreateTextInformation(this.Value, this.Value);

				return this._textInformation.Value;
			}
		}

		public virtual T Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

		#endregion
	}
}