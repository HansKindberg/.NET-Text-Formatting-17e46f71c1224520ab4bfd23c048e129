namespace HansKindberg.Text.Formatting.Yaml.Configuration
{
	public enum DocumentNotation
	{
		/// <summary>
		/// Keeps the original notation.
		/// </summary>
		None,

		/// <summary>
		/// Separates documents with document-end, "...", if there are multiple documents.
		/// </summary>
		Minimal,
		ForceDocumentEnd,
		ForceDocumentStart,
		ForceDocumentEndAndDocumentStart
	}
}