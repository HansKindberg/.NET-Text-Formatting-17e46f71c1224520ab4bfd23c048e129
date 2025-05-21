namespace HansKindberg.Text.Formatting.Diagnostics
{
	public readonly struct LinePositionSpan(LinePosition start, LinePosition end)
	{
		#region Properties

		public LinePosition End { get; } = end;
		public LinePosition Start { get; } = start;

		#endregion
	}
}