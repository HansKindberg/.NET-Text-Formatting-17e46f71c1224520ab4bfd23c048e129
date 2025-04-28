namespace YamlDotNetTests
{
	public static class Global
	{
		#region Fields

		public static readonly DirectoryInfo ProjectDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!)).Parent!.Parent!.Parent!;

		#endregion
	}
}