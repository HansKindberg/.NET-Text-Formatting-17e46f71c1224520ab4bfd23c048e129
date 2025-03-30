namespace HansKindberg.Text.Formatting.Configuration
{
	public class IndentationOptions
	{
		#region Properties

		public char Character { get; set; } = '\t';
		public bool Enabled { get; set; } = true;
		public byte Size { get; set; } = 1;

		#endregion
	}
}