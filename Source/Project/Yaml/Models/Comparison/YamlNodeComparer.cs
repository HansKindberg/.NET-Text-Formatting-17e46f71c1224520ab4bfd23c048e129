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

			if(x.GetType() != y.GetType())
				throw new InvalidOperationException("The nodes must be of the same type.");

			var compare = x is IYamlSequenceItemNode ? this.CompareSequences(x, y) : this.CompareNodes(x, y);

			if(compare != 0)
				return compare;

			// We use stable sorting if the nodes are considered equal.
			return x.Index.CompareTo(y.Index);
		}

		protected internal virtual int Compare(string x, string y, AlphabeticalSortingOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(!options.Enabled)
				throw new ArgumentException("Sorting not enabled.", nameof(options));

			return options.Direction == ListSortDirection.Descending ? string.Compare(y, x, options.Comparison) : string.Compare(x, y, options.Comparison);
		}

		protected internal virtual int CompareNodes(IYamlNode x, IYamlNode y)
		{
			if(!this.Options.NodeSorting.Enabled)
				return 0;

			return this.Compare(x.ToString(this.Options), y.ToString(this.Options), this.Options.NodeSorting);
		}

		protected internal virtual int CompareSequences(IYamlNode x, IYamlNode y)
		{
			if(!this.Options.SequenceSorting.Enabled)
				return 0;

			var firstChild = x.Children.FirstOrDefault();
			var secondChild = y.Children.FirstOrDefault();

			if(firstChild == null || secondChild == null)
				return 0;

			return this.Compare(firstChild.ToString(this.Options), secondChild.ToString(this.Options), this.Options.SequenceSorting);
		}

		#endregion
	}
}