using HansKindberg.Text.Formatting.Yaml.Configuration;

namespace HansKindberg.Text.Formatting.Yaml.Models.Comparison
{
	public class YamlDocumentComparer(YamlFormatOptions options) : IComparer<IYamlDocument>
	{
		#region Properties

		protected internal virtual YamlFormatOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

		#endregion

		#region Methods

		public virtual int Compare(IYamlDocument x, IYamlDocument y)
		{
			if(x == null)
				throw new ArgumentNullException(nameof(x));

			if(y == null)
				throw new ArgumentNullException(nameof(y));

			if(this.Options.DocumentSorting.Enabled)
				throw new NotSupportedException("Document sorting is enabled but we haven't figured it out yet how to sort.");

			return x.Index.CompareTo(y.Index);
		}

		#endregion
	}
}