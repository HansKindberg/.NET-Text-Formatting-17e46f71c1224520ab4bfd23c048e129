using System.ComponentModel;
using HansKindberg.Text.Formatting.Xml;
using HansKindberg.Text.Formatting.Xml.Comparison;
using Moq;

namespace UnitTests.Xml.Comparison
{
	public class XmlAttributeComparerTest
	{
		#region Methods

		[Fact]
		public async Task Compare_Descending_Test()
		{
			var options = new XmlAttributeFormat
			{
				AlphabeticalSortDirection = ListSortDirection.Descending
			};

			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(1, xmlAttributeComparer.Compare(await this.CreateXmlAttribute(name: "a"), await this.CreateXmlAttribute(name: "b")));

			options.AlphabeticalSortDirection = ListSortDirection.Ascending;

			Assert.Equal(-1, xmlAttributeComparer.Compare(await this.CreateXmlAttribute(name: "a"), await this.CreateXmlAttribute(name: "b")));
		}

		[Fact]
		public async Task Compare_IfBothParametersAreNull_ShouldReturnZero()
		{
			await Task.CompletedTask;

			var options = new XmlAttributeFormat();
			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(0, xmlAttributeComparer.Compare(null, null));
		}

		[Fact]
		public async Task Compare_IfFirstParameterIsNull_ShouldReturnMinusOne()
		{
			var options = new XmlAttributeFormat();
			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(-1, xmlAttributeComparer.Compare(null, await this.CreateXmlAttribute()));
		}

		[Fact]
		public async Task Compare_IfFirstParameterIsPinned_ShouldReturnMinusOne()
		{
			const string pinned = "@pinned";

			var options = new XmlAttributeFormat();
			options.Pinned.Add(pinned);

			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(-1, xmlAttributeComparer.Compare(await this.CreateXmlAttribute(matches: options.Pinned), await this.CreateXmlAttribute()));
		}

		[Fact]
		public async Task Compare_IfSecondParameterIsNull_ShouldReturnOne()
		{
			var options = new XmlAttributeFormat();
			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(1, xmlAttributeComparer.Compare(await this.CreateXmlAttribute(), null));
		}

		[Fact]
		public async Task Compare_IfSecondParameterIsPinned_ShouldReturnOne()
		{
			const string pinned = "@pinned";

			var options = new XmlAttributeFormat();
			options.Pinned.Add(pinned);

			var xmlAttributeComparer = new XmlAttributeComparer(options);

			Assert.Equal(1, xmlAttributeComparer.Compare(await this.CreateXmlAttribute(), await this.CreateXmlAttribute(matches: options.Pinned)));
		}

		protected internal virtual async Task<IXmlAttribute> CreateXmlAttribute(int initialIndex = 0, IList<string>? matches = null, string? name = null, string? value = null)
		{
			await Task.CompletedTask;

			var xmlAttributeMock = new Mock<IXmlAttribute>();

			xmlAttributeMock.Setup(xmlAttribute => xmlAttribute.InitialIndex).Returns(initialIndex);
			xmlAttributeMock.Setup(xmlAttribute => xmlAttribute.Name).Returns(name);
			xmlAttributeMock.Setup(xmlAttribute => xmlAttribute.Value).Returns(value);

			xmlAttributeMock.Setup(xmlAttribute => xmlAttribute.Matches(It.IsAny<string>())).Returns(false);

			foreach(var match in matches ?? [])
			{
				xmlAttributeMock.Setup(xmlAttribute => xmlAttribute.Matches(match)).Returns(true);
			}

			return xmlAttributeMock.Object;
		}

		#endregion
	}
}