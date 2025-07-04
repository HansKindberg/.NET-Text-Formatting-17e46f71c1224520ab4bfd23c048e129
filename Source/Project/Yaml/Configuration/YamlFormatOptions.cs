using HansKindberg.Text.Formatting.Configuration;
using HansKindberg.Text.Formatting.Yaml.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Configuration
{
	public class YamlFormatOptions
	{
		#region Properties

		public char AnchorAliasPrefix { get; set; } = '*';
		public char AnchorPrefix { get; set; } = '&';
		public string CommentPrefix { get; set; } = "# ";
		public DocumentNotation DocumentNotation { get; set; }

		/// <summary>
		/// Document sorting.
		/// </summary>
		public AlphabeticalSortingOptions DocumentSorting { get; set; } = new() { Enabled = false };

		public string EndOfDocument { get; set; } = "...";
		public string ErrorPrefix { get; set; } = "ERROR: ";

		public IndentationOptions Indentation { get; set; } = new()
		{
			Character = ' ',
			Enabled = true,
			Size = 2
		};

		public Quotation? KeyQuotation { get; set; }

		/// <summary>
		/// The key-value-separator, e.g. "a: b", the colon.
		/// </summary>
		public char KeySuffix { get; set; } = ':';

		public NamingConvention? NamingConvention { get; set; }
		public string NewLine { get; set; } = Environment.NewLine;

		/// <summary>
		/// Scalar sorting.
		/// </summary>
		public AlphabeticalSortingOptions ScalarSorting { get; set; } = new();

		public char SequenceCharacter { get; set; } = '-';

		/// <summary>
		/// Sequence sorting.
		/// </summary>
		public AlphabeticalSortingOptions SequenceSorting { get; set; } = new() { Enabled = false };

		public char Space { get; set; } = ' ';
		public string StartOfDocument { get; set; } = "---";
		public string TagDirectivePrefix { get; set; } = "%TAG";
		public char TagPrefix { get; set; } = '!';
		public bool TrimComments { get; set; } = true;
		public Quotation? ValueQuotation { get; set; }
		public string VersionDirectivePrefix { get; set; } = "%YAML";

		#endregion
	}
}