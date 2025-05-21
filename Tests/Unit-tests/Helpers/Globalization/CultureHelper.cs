using System.Globalization;

namespace UnitTests.Helpers.Globalization
{
	public static class CultureHelper
	{
		#region Methods

		public static void SetTestCulture()
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-001");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");
		}

		#endregion
	}
}