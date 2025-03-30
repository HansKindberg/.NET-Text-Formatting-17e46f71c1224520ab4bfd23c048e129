namespace HansKindberg.Text.Formatting.Comparison
{
	public abstract class Comparer
	{
		#region Properties

		protected internal virtual bool NullIsLeast { get; set; } = true;

		#endregion

		#region Methods

		protected internal virtual int? NullCompare(object? first, object? second)
		{
			return this.NullCompare(first, second, this.NullIsLeast);
		}

		protected internal virtual int? NullCompare(object? first, object? second, bool nullIsLeast)
		{
			var compare = this.NullCompareWhenNullIsLeast(first, second);

			if(compare != null)
				return compare.Value * (nullIsLeast ? 1 : -1);

			return null;
		}

		protected internal virtual int? NullCompareWhenNullIsLeast(object? first, object? second)
		{
			if(first == null)
			{
				if(second == null)
					return 0;

				return -1;
			}

			if(second == null)
				return 1;

			return null;
		}

		protected internal virtual int PinIndexCompare(int? firstPinIndex, int? secondPinIndex)
		{
			var compare = this.NullCompare(firstPinIndex, secondPinIndex, false);

			// ReSharper disable PossibleInvalidOperationException

			return compare ?? firstPinIndex!.Value.CompareTo(secondPinIndex!.Value);

			// ReSharper restore PossibleInvalidOperationException
		}

		#endregion
	}
}