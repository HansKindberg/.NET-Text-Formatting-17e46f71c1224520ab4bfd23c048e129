namespace HansKindberg.Text.Formatting
{
	/// <inheritdoc />
	public class IndentableFormat : IIndentableFormat
	{
		#region Properties

		public virtual bool Indent { get; set; }
		public virtual string IndentString { get; set; } = @"\t";
		public virtual string NewLineString { get; set; } = Environment.NewLine;

		#endregion
	}
}