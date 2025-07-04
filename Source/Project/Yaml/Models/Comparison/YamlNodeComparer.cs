using System.ComponentModel;
using HansKindberg.Text.Formatting.Configuration;
using HansKindberg.Text.Formatting.Yaml.Configuration;

namespace HansKindberg.Text.Formatting.Yaml.Models.Comparison
{
	public class YamlNodeComparer(YamlFormatOptions options) : IComparer<IYamlNode>
	{
		#region Properties

		protected internal virtual YamlFormatOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

		#endregion

		#region Methods

		public virtual int Compare(IYamlNode x, IYamlNode y)
		{
			if(x == null)
				throw new ArgumentNullException(nameof(x));

			if(y == null)
				throw new ArgumentNullException(nameof(y));

			if(x.Level != y.Level)
				throw new InvalidOperationException("The nodes must have the same level.");

			if(x.Parent != y.Parent)
				throw new InvalidOperationException("The nodes must have the same parent.");

			////var sequence = x.Parent is IYamlSequenceNode { Sequence: true };

			////if(sequence)
			////{
			////	if(this.Options.SequenceSorting.Enabled)
			////		return this.Compare(x, y, this.Options, this.Options.SequenceSorting);

			////	return 0;
			////}

			//if(this.Options.ScalarSorting.Enabled)
			//	return this.Compare(x, y, this.Options, this.Options.ScalarSorting);

			return x.Index.CompareTo(y.Index);
		}

		protected internal virtual int Compare(string x, string y, AlphabeticalSortingOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(!options.Enabled)
				return 0;

			var comparison = string.Compare(x, y, options.Comparison);

			if(comparison == 0)
				return 0;

			if(options.Direction == ListSortDirection.Ascending)
				return comparison;

			return -comparison;
		}

		protected internal virtual int Compare(IYamlNode x, IYamlNode y, YamlFormatOptions options, AlphabeticalSortingOptions sortingOptions)
		{
			return 0;

			//////////////////////////////////////////if(x is not IYamlDocumentNode)
			//////////////////////////////////////////	return this.Compare(x.ToString(options), y.ToString(options), sortingOptions);

			//////////////////////////////////////////var first = x.Children.FirstOrDefault()?.ToString(options) ?? string.Empty;
			//////////////////////////////////////////var second = y.Children.FirstOrDefault()?.ToString(options) ?? string.Empty;

			//////////////////////////////////////////return this.Compare(first, second, sortingOptions);
		}

		#endregion
	}
}