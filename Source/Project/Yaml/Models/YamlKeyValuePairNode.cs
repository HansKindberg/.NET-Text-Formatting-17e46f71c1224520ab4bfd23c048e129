using HansKindberg.Text.Formatting.Diagnostics;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlKeyValuePairNode<TKey, TValue>(TKey key, TValue value) : YamlNode where TKey : ParsingEvent where TValue : ParsingEvent
	{
		#region Fields

		private TextInformation? _textInformation;

		#endregion

		#region Properties

		public virtual TKey Key { get; } = key ?? throw new ArgumentNullException(nameof(key));

		public override TextInformation TextInformation
		{
			get
			{
				this._textInformation ??= this.CreateTextInformation(this.Key, this.Value);
				return this._textInformation.Value;
			}
		}

		public virtual TValue Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

		#endregion
	}
}