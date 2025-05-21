namespace HansKindberg.Text.Formatting.Collections.Generic.Extensions
{
	public static class ListExtension
	{
		#region Methods

		public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
		{
			list.ExecuteAction(concreteList => { concreteList.AddRange(collection); });
		}

		private static void ExecuteAction<T>(this IList<T> list, Action<List<T>> action)
		{
			if(list is null)
				throw new ArgumentNullException(nameof(list));

			var emptyAndFill = false;

			if(list is not List<T> concreteList)
			{
				concreteList = [.. list];
				emptyAndFill = true;
			}

			action(concreteList);

			if(!emptyAndFill)
				return;

			list.Clear();

			foreach(var item in concreteList)
			{
				list.Add(item);
			}
		}

		public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
		{
			list.ExecuteAction(concreteList => { concreteList.Sort(comparison); });
		}

		public static void Sort<T>(this IList<T> list, IComparer<T> comparer)
		{
			list.ExecuteAction(concreteList => { concreteList.Sort(comparer); });
		}

		#endregion
	}
}