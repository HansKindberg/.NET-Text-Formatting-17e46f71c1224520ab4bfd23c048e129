namespace HansKindberg.Text.Formatting.Diagnostics
{
	public readonly struct LinePosition(long character, long line)
	{
		#region Properties

		public long Character { get; } = character;
		public long Line { get; } = line;

		#endregion
	}
}