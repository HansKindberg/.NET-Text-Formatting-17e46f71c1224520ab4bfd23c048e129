using System.ComponentModel;
using HansKindberg.Text.Formatting.Xml;

namespace UnitTests.Xml
{
	public class XmlAttributeFormatTest
	{
		#region Methods

		[Fact]
		public async Task AlphabeticalSortDirection_ShouldReturnAscendingByDefault()
		{
			await Task.CompletedTask;
			Assert.Equal(ListSortDirection.Ascending, new XmlAttributeFormat().AlphabeticalSortDirection);
		}

		[Fact]
		public async Task NameComparison_ShouldReturnOrdinalByDefault()
		{
			await Task.CompletedTask;
			Assert.Equal(StringComparison.Ordinal, new XmlAttributeFormat().NameComparison);
		}

		[Fact]
		public async Task Pinned_ShouldReturnAnEmptyCollectionByDefault()
		{
			await Task.CompletedTask;
			var xmlAttributeFormat = new XmlAttributeFormat();
			Assert.NotNull(xmlAttributeFormat.Pinned);
			Assert.False(xmlAttributeFormat.Pinned.Any());
		}

		[Fact]
		public async Task SortAlphabetically_ShouldReturnTrueByDefault()
		{
			await Task.CompletedTask;
			Assert.True(new XmlAttributeFormat().SortAlphabetically);
		}

		[Fact]
		public async Task ValueComparison_ShouldReturnOrdinalByDefault()
		{
			await Task.CompletedTask;
			Assert.Equal(StringComparison.Ordinal, new XmlAttributeFormat().ValueComparison);
		}

		#endregion
	}
}