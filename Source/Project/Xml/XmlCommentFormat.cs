namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc />
	public class XmlCommentFormat : IXmlCommentFormat
	{
		#region Properties

		public virtual bool BelongsToPrevious { get; set; }
		public virtual CommentMode Mode { get; set; } = CommentMode.None;
		public virtual bool Omit { get; set; }

		#endregion
	}
}