namespace HansKindberg.Text.Formatting.Diagnostics
{
	public readonly struct TextInformation(LinePositionSpan startLine, LinePositionSpan endLine)
	{
		#region Properties

		public LinePositionSpan EndLine { get; } = endLine;
		public LinePositionSpan StartLine { get; } = startLine;

		#endregion
	}
}