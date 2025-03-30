using HansKindberg.Text.Formatting.Comparison;
using Moq;

namespace UnitTests.Comparison
{
	public class ComparerTest
	{
		#region Methods

		protected internal virtual async Task<Comparer> CreateComparer()
		{
			await Task.CompletedTask;

			return new Mock<Comparer> { CallBase = true }.Object;
		}

		[Fact]
		public async Task PinIndexCompare_Test()
		{
			var comparer = await this.CreateComparer();
			Assert.Equal(0, comparer.PinIndexCompare(null, null));

			Assert.Equal(-1, comparer.PinIndexCompare(-20, null));
			Assert.Equal(-1, comparer.PinIndexCompare(-1, null));
			Assert.Equal(-1, comparer.PinIndexCompare(0, null));
			Assert.Equal(-1, comparer.PinIndexCompare(1, null));
			Assert.Equal(-1, comparer.PinIndexCompare(20, null));

			Assert.Equal(1, comparer.PinIndexCompare(null, -20));
			Assert.Equal(1, comparer.PinIndexCompare(null, -1));
			Assert.Equal(1, comparer.PinIndexCompare(null, 0));
			Assert.Equal(1, comparer.PinIndexCompare(null, 1));
			Assert.Equal(1, comparer.PinIndexCompare(null, 20));
		}

		#endregion
	}
}