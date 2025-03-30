using System.Globalization;
using System.Xml;
using HansKindberg.Text.Formatting.Xml;

namespace UnitTests.Xml
{
	public class XmlParserTest
	{
		#region Constructors

		public XmlParserTest()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
		}

		#endregion

		#region Methods

		[Fact]
		public async Task Parse_IfTheValueContainsANestedXmlDeclaration_ShouldThrowAnInvalidOperationException()
		{
			const string value = "<root><?xml version=\"1.0\" encoding=\"utf-8\"?></root>";
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await new XmlParser().Parse(value));
			Assert.NotNull(exception);
			Assert.Equal($"Could not parse the value \"{value}\" to xml-fragment.", exception.Message);
			var xmlException = exception.InnerException as XmlException;
			Assert.NotNull(xmlException);
#if NETFRAMEWORK
			const string whitespace = "white space";
#else
			const string whitespace = "whitespace";
#endif
			Assert.Equal($"Unexpected XML declaration. The XML declaration must be the first node in the document, and no {whitespace} characters are allowed to appear before it. Line 1, position 9.", xmlException.Message);
		}

		[Fact]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyXmlFragment()
		{
			var xmlFragment = await new XmlParser().Parse(string.Empty);
			Assert.NotNull(xmlFragment);
			Assert.False(xmlFragment.Nodes.Any());
		}

		[Fact]
		public async Task Parse_IfTheValueParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await new XmlParser().Parse(null!));
			Assert.Equal("value", argumentNullException.ParamName);
		}

		[Theory]
		[InlineData("     ", 1)]
		[InlineData("     \n     \n     ", 1)]
		[InlineData("     \r\n     \r\n     ", 1)]
		[InlineData("     \n     \r\n     \n     \r\n     ", 1)]
		[InlineData("     <root />     ", 3)]
		[InlineData("     \n     \r\n     <root />     \n     \r\n     ", 3)]
		[InlineData("     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     ", 7)]
		[InlineData("     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     ", 9)]
		[InlineData("<root />", 1)]
		[InlineData("<p>", 1)]
		[InlineData("<div>", 1)]
		[InlineData("<p><div>", 2)]
		[InlineData("<div><p>", 1)]
		[InlineData("<p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>", 10)]
		[InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>", 11)]
		[InlineData("  \r\n  <?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>  ", 12)]
		[InlineData("<a:b>", 1)]
		[InlineData("<a:b />", 1)]
		[InlineData("<a:b></a:b>", 1)]
		[InlineData("<div a:b=\"c\">", 1)]
		[InlineData("<div a:b=\"c\" />", 1)]
		[InlineData("<div a:b=\"c\"></div>", 1)]
		[InlineData("<a:b c:d=\"e\">", 1)]
		[InlineData("<a:b c:d=\"e\" />", 1)]
		[InlineData("<a:b c:d=\"e\"></a:b>", 1)]
		public async Task Parse_ShouldWorkProperly(string value, int expectedNumberOfNodes)
		{
			var xmlFragment = await new XmlParser().Parse(value);
			Assert.NotNull(xmlFragment);
			Assert.Equal(expectedNumberOfNodes, xmlFragment.Nodes.Count);
		}

		[Theory]
		[InlineData("     ", "     ")]
		[InlineData("     \n     \r\n     \n     \r\n     ", "     \n     \r\n     \n     \r\n     ")]
		[InlineData("     \n     \r\n     <root/>     \n     \r\n     <root />     \n     \r\n     <root   />     \n     \r\n     ", "     \n     \r\n     <root></root>     \n     \r\n     <root></root>     \n     \r\n     <root></root>     \n     \r\n     ")]
		[InlineData("     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     <root />     \n     \r\n     ", "     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \n     \r\n     <root></root>     \n     \r\n     <root></root>     \n     \r\n     <root></root>     \n     \r\n     ")]
		[InlineData("     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \n     \r\n     <root>     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     <root />     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     <root     />     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     ", "     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \n     \r\n     <root>     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     <root></root>     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     <root></root>     \n     <?xml version=\"1.0\" encoding=\"utf-8\"?>     \r\n     </root>")]
		[InlineData("<root />", "<root></root>")]
		[InlineData("<p>", "<p></p>")]
		[InlineData("<div>", "<div></div>")]
		[InlineData("<p><div>", "<p></p><div></div>")]
		[InlineData("<div><p>", "<div><p></p></div>")]
		[InlineData("<p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>", "<p>Text</p><div>Content</div><p></p><div></div><p></p><div></div><p></p><div></div><p></p><div></div>")]
		[InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>", "<?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p></p><div></div><p></p><div></div><p></p><div></div>")]
		[InlineData("  \r\n  <?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p /><div /><p/><div/><p><div>  ", "  \r\n  <?xml version=\"1.0\" encoding=\"utf-8\"?><p>Text</p><div>Content</div><p></p><div></div><p></p><div></div><p></p><div></div><p></p><div>  </div>")]
		[InlineData("<a:b>", "<a:b></a:b>")]
		[InlineData("<div a:b=\"c\" />", "<div a:b=\"c\"></div>")]
		public async Task Repair_ShouldWorkProperly(string value, string expectedRepairedValue)
		{
			var repairedValue = await new XmlParser().Repair(value);
			Assert.Equal(expectedRepairedValue, repairedValue);
		}

		#endregion
	}
}