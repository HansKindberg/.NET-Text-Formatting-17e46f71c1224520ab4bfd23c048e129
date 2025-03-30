namespace Shared.Extensions
{
	public static class StringExtension
	{
		#region Methods

		public static string ResolveNewLine(this string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			return value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
		}

		#endregion
	}
}