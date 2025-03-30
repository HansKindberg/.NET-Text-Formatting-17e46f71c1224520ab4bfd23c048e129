using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using HansKindberg.Text.Formatting.Comparison;

namespace HansKindberg.Text.Formatting.Xml.Comparison
{
	public class XmlAttributeComparer(IXmlAttributeFormat options) : Comparer, IComparer<IXmlAttribute>
	{
		#region Properties

		protected internal virtual IXmlAttributeFormat Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

		#endregion

		#region Methods

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
		public virtual int Compare(IXmlAttribute? x, IXmlAttribute? y)
		{
			// ReSharper disable All

			var nullableCompare = this.NullCompare(x, y);

			if(nullableCompare != null)
				return nullableCompare.Value;

			var compare = this.PinIndexCompare(this.GetPinIndex(x), this.GetPinIndex(y));

			if(compare != 0)
				return compare;

			if(this.Options.SortAlphabetically)
			{
				compare = string.Compare(x!.Name, y!.Name, this.Options.NameComparison);

				if(compare == 0)
					compare = string.Compare(x.Value, y.Value, this.Options.ValueComparison);

				if(compare != 0)
					return this.Options.AlphabeticalSortDirection == ListSortDirection.Ascending ? compare : compare * -1;
			}

			return x!.InitialIndex.CompareTo(y!.InitialIndex);

			// ReSharper restore All
		}

		protected internal virtual int? GetPinIndex(IXmlAttribute? attribute)
		{
			// ReSharper disable InvertIf
			if(attribute != null)
			{
				for(var i = 0; i < this.Options.Pinned.Count(); i++)
				{
					if(attribute.Matches(this.Options.Pinned.ElementAt(i)))
						return i;
				}
			}
			// ReSharper restore InvertIf

			return null;
		}

		#endregion
	}
}