namespace HansKindberg.Text.Formatting.Xml
{
	public interface IXmlFormat : IIndentableFormat
	{
		#region Properties

		IXmlAttributeFormat Attribute { get; }
		IXmlCommentFormat Comment { get; }
		IXmlElementFormat Element { get; }

		#endregion
	}
}