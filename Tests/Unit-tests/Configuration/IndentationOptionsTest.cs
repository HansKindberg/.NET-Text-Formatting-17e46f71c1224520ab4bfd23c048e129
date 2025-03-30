namespace UnitTests.Configuration
{
	public class IndentationOptionsTest
	{
		#region Methods

		[Theory]
		[InlineData('\t', 0, "")]
		[InlineData('\t', 1, "\t")]
		[InlineData('\t', 2, "\t\t")]
		[InlineData('\t', 3, "\t\t\t")]
		[InlineData('\t', 4, "\t\t\t\t")]
		public async Task CalculateIndentationString_Test(char character, byte size, string expected)
		{
			await Task.CompletedTask;

			Assert.Equal(expected, new string(character, size));
		}

		#endregion
	}
}