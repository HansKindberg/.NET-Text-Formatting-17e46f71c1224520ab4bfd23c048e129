namespace HansKindberg.Text.Formatting.Xml
{
	/// <inheritdoc cref="IXmlFormat" />
	public class XmlFormat : IndentableFormat, IXmlFormat
	{
		#region Properties

		IXmlAttributeFormat IXmlFormat.Attribute => this.Attribute;
		public virtual XmlAttributeFormat Attribute { get; set; } = new XmlAttributeFormat();
		IXmlCommentFormat IXmlFormat.Comment => this.Comment;
		public virtual XmlCommentFormat Comment { get; set; } = new XmlCommentFormat();
		IXmlElementFormat IXmlFormat.Element => this.Element;
		public virtual XmlElementFormat Element { get; set; } = new XmlElementFormat();

		#endregion
	}
}