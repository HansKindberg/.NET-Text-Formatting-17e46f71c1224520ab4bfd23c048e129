using HansKindberg.Text.Formatting.Diagnostics;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlDocumentNode(DocumentStart start, DocumentEnd end) : YamlNode
	{
		#region Fields

		private TextInformation? _textInformation;

		#endregion

		#region Properties

		public virtual DocumentEnd End { get; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual DocumentStart Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

		public override TextInformation TextInformation
		{
			get
			{
				this._textInformation ??= this.CreateTextInformation(this.Start, this.End);

				return this._textInformation.Value;
			}
		}

		#endregion
	}
}