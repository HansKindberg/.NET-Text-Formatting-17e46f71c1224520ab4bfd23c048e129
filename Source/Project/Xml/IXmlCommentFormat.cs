namespace HansKindberg.Text.Formatting.Xml
{
	public interface IXmlCommentFormat
	{
		#region Properties

		/// <summary>
		/// If the comment belongs to the previous xml-node, sibling, or not. Used when sorting nodes.
		/// If false, the comment belongs to the initially next node, and will during sort end up before the initially next node even if it changes location.
		/// If true, the comment belongs to the initially previous node, and will during sort end up after the initially previous node even if it changes location. 
		/// </summary>
		bool BelongsToPrevious { get; }

		/// <summary>
		/// Affects how the comments are formatted.
		/// </summary>
		CommentMode Mode { get; }

		/// <summary>
		/// Do not include comments when formatting xml. Removes existing comments.
		/// </summary>
		bool Omit { get; }

		#endregion
	}
}