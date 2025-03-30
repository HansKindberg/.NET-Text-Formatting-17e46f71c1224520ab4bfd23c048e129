namespace Shared.Extensions
{
	public static class StringExtension
	{
		#region Methods

		/// <summary>
		/// New lines are different on different platforms. E.g. "\n" on Linux and "\r\n" on Windows. This method resolves it.
		/// </summary>
		public static string ResolveNewLine(this string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			return value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
		}

		#endregion
	}
}