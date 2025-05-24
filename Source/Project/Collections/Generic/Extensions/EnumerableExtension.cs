namespace HansKindberg.Text.Formatting.Collections.Generic.Extensions
{
	public static class EnumerableExtension
	{
		#region Methods

		public static int IndexOf<T>(this IEnumerable<T> enumerable, T instance)
		{
			var index = 0;

			foreach(var item in enumerable)
			{
				if(Equals(item, instance))
					return index;

				index++;
			}

			return -1;
		}

		#endregion
	}
}