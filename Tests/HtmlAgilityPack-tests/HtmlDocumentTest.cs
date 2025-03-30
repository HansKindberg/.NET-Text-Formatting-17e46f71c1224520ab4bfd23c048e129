using System.Text;
using HtmlAgilityPack;

namespace HtmlAgilityPackTests
{
	public class HtmlDocumentTest
	{
		#region Methods

		[Theory]
		[InlineData("<div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><div></div></root>")]
		[InlineData("<div />", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><div></div></root>")]
		[InlineData("<div></div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><div></div></root>")]
		[InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?><div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><!--l version=\"1.0\" encoding=\"utf-8--><div></div></root>")]
		[InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?><div />", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><!--l version=\"1.0\" encoding=\"utf-8--><div></div></root>")]
		[InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?><div></div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><root><!--l version=\"1.0\" encoding=\"utf-8--><div></div></root>")]
		public async Task LoadHtml_AddRootElement_Test(string html, string expectedOuterHtml)
		{
			await Task.CompletedTask;

			var htmlDocument = new HtmlDocument
			{
				OptionOutputAsXml = true
			};

			htmlDocument.LoadHtml($"<root>{html}</root>");

			Assert.Equal(ResolveEncoding(expectedOuterHtml), htmlDocument.DocumentNode.OuterHtml);
		}

		[Theory]
		[InlineData("<link />", "<link>")]
		[InlineData("<link></link>", "<link>")]
		[InlineData("<link>", "<link>")]
		[InlineData("<link />", "<link></link>", true)]
		[InlineData("<link></link>", "<link></link>", true)]
		[InlineData("<link>", "<link></link>", true)]
		public async Task LoadHtml_AnotherLinkElement_Test(string html, string expectedOuterHtml, bool clearElementsFlags = false)
		{
			await Task.CompletedTask;

			var elementsFlags = new Dictionary<string, HtmlElementFlag>();

			if(clearElementsFlags)
			{
				elementsFlags = new Dictionary<string, HtmlElementFlag>(HtmlNode.ElementsFlags);
				HtmlNode.ElementsFlags.Clear();
			}

			var htmlDocument = new HtmlDocument
			{
				BackwardCompatibility = false,
				OptionAutoCloseOnEnd = true
			};
			htmlDocument.LoadHtml(html);
			Assert.Equal(expectedOuterHtml, htmlDocument.DocumentNode.OuterHtml);

			if(clearElementsFlags)
			{
				foreach(var elementsFlag in elementsFlags)
				{
					HtmlNode.ElementsFlags.Add(elementsFlag.Key, elementsFlag.Value);
				}
			}
		}

		[Theory]
		[InlineData("<div>", "<div></div>", false, false)]
		[InlineData("<div>", "<div></div>", true, false)]
		[InlineData("<div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><div></div>", false, true)]
		[InlineData("<div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><div></div>", true, true)]
		[InlineData("<p><div>", "<p><div></div>", false, false)]
		[InlineData("<p><div>", "<p><div></div>", true, false)]
		[InlineData("<p><div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><span><p></p><div></div></span>", false, true)]
		[InlineData("<p><div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><span><p></p><div></div></span>", true, true)]
		public async Task LoadHtml_AutoCloseOnEndTogetherWithOutputAsXml_Test(string html, string expectedOuterHtml, bool autoCloseOnEnd, bool outputAsXml)
		{
			await Task.CompletedTask;

			var htmlDocument = new HtmlDocument
			{
				OptionAutoCloseOnEnd = autoCloseOnEnd,
				OptionOutputAsXml = outputAsXml
			};

			htmlDocument.LoadHtml(html);

			Assert.Equal(ResolveEncoding(expectedOuterHtml), htmlDocument.DocumentNode.OuterHtml);
		}

		[Theory]
		[InlineData("<p />", "<p></p>")]
		[InlineData("<p></p>", "<p></p>")]
		[InlineData("<p>", "<p></p>", 1)]
		[InlineData("<p />", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><p></p>", 0, true)]
		[InlineData("<p></p>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><p></p>", 0, true)]
		[InlineData("<p>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><p></p>", 1, true)]
		[InlineData("<div />", "<div></div>")]
		[InlineData("<div></div>", "<div></div>")]
		[InlineData("<div>", "<div></div>", 1)]
		[InlineData("<div />", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><div></div>", 0, true)]
		[InlineData("<div></div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><div></div>", 0, true)]
		[InlineData("<div>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><div></div>", 1, true)]
		public async Task LoadHtml_Element_Test(string html, string expectedOuterHtml, int expectedNumberOfParseErrors = 0, bool optionOutputAsXml = false)
		{
			await Task.CompletedTask;

			var htmlDocument = new HtmlDocument
			{
				OptionOutputAsXml = optionOutputAsXml
			};

			htmlDocument.LoadHtml(html);

			Assert.Equal(expectedNumberOfParseErrors, htmlDocument.ParseErrors.Count());
			Assert.Equal(ResolveEncoding(expectedOuterHtml), htmlDocument.DocumentNode.OuterHtml);
		}

		[Theory]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\" />", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\"></link>", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">", 1)]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\">", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\" />", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><link href=\"/Style/Site.css\" rel=\"stylesheet\" />", 0, true)]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\"></link>", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><link href=\"/Style/Site.css\" rel=\"stylesheet\" />", 1, true)]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\">", "<?xml version=\"1.0\" encoding=\"{ENCODING}\"?><link href=\"/Style/Site.css\" rel=\"stylesheet\" />", 0, true)]
		public async Task LoadHtml_LinkElement_Test(string html, string expectedOuterHtml, int expectedNumberOfParseErrors = 0, bool optionOutputAsXml = false)
		{
			await Task.CompletedTask;

			var htmlDocument = new HtmlDocument
			{
				OptionOutputAsXml = optionOutputAsXml
			};

			htmlDocument.LoadHtml(html);

			Assert.Equal(expectedNumberOfParseErrors, htmlDocument.ParseErrors.Count());
			Assert.Equal(ResolveEncoding(expectedOuterHtml), htmlDocument.DocumentNode.OuterHtml);
		}

		[Theory]
		[InlineData("<div />", "<div></div>")]
		[InlineData("<div></div>", "<div></div>")]
		[InlineData("<div>", "<div></div>")]
		[InlineData("<p />", "<p></p>")]
		[InlineData("<p></p>", "<p></p>")]
		[InlineData("<p>", "<p></p>")]
		public async Task LoadHtml_OuterHtml_Test(string html, string expectedOuterHtml)
		{
			await Task.CompletedTask;

			var htmlDocument = new HtmlDocument
			{
				BackwardCompatibility = false,
				OptionAutoCloseOnEnd = true
			};
			htmlDocument.LoadHtml(html);
			Assert.Equal(expectedOuterHtml, htmlDocument.DocumentNode.OuterHtml);
		}

		[Fact]
		public async Task LoadHtml_ShouldHandleNamespaceAttributes()
		{
			await Task.CompletedTask;

			const string html = "<element xmlns:namespace=\"http://example.com/ns\" my-namespace:name=\"value\" />";
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			Assert.Single(htmlDocument.DocumentNode.ChildNodes);
			var element = htmlDocument.DocumentNode.ChildNodes[0];
			Assert.Equal(2, element.Attributes.Count);
			Assert.Equal("xmlns:namespace", element.Attributes[0].Name);
			Assert.Equal("http://example.com/ns", element.Attributes[0].Value);
			Assert.Equal("my-namespace:name", element.Attributes[1].Name);
			Assert.Equal("value", element.Attributes[1].Value);
		}

		[Fact]
		public async Task Properties_DefaultValue_Test()
		{
			await Task.CompletedTask;

			Assert.Equal(int.MaxValue, HtmlDocument.MaxDepthLevel);
			Assert.Null(HtmlDocument.DefaultBuilder);
			Assert.True(HtmlDocument.DisableBehaviorTagP);

			var htmlDocument = new HtmlDocument();

			Assert.True(htmlDocument.BackwardCompatibility);
			Assert.False(htmlDocument.DisableImplicitEnd);
			Assert.False(htmlDocument.DisableServerSideCode);
			Assert.Null(htmlDocument.GlobalAttributeValueQuote);
			Assert.False(htmlDocument.OptionAddDebuggingAttributes);
			Assert.False(htmlDocument.OptionAutoCloseOnEnd);
			Assert.True(htmlDocument.OptionCheckSyntax);
			Assert.False(htmlDocument.OptionComputeChecksum);
			Assert.Equal(Encoding.Default, htmlDocument.OptionDefaultStreamEncoding);
			Assert.False(htmlDocument.OptionDefaultUseOriginalName);
			Assert.False(htmlDocument.OptionEmptyCollection);
			Assert.False(htmlDocument.OptionEnableBreakLineForInnerText);
			Assert.False(htmlDocument.OptionExtractErrorSourceText);
			Assert.Equal(100, htmlDocument.OptionExtractErrorSourceTextMaxLength);
			Assert.False(htmlDocument.OptionFixNestedTags);
			Assert.Equal(0, htmlDocument.OptionMaxNestedChildNodes);
			Assert.False(htmlDocument.OptionOutputAsXml);
			Assert.False(htmlDocument.OptionOutputOptimizeAttributeValues);
			Assert.False(htmlDocument.OptionOutputOriginalCase);
			Assert.False(htmlDocument.OptionOutputUpperCase);
			Assert.False(htmlDocument.OptionPreserveXmlNamespaces);
			Assert.True(htmlDocument.OptionReadEncoding);
			Assert.Null(htmlDocument.OptionStopperNodeName);
			Assert.False(htmlDocument.OptionTreatCDataBlockAsComment);
			Assert.True(htmlDocument.OptionUseIdAttribute);
			Assert.False(htmlDocument.OptionWriteEmptyNodes);
			Assert.False(htmlDocument.OptionWriteEmptyNodesWithoutSpace);
			Assert.False(htmlDocument.OptionXmlForceOriginalComment);
			Assert.Null(htmlDocument.ParseExecuting);
			Assert.Null(htmlDocument.Text);
		}

		private static string ResolveEncoding(string html)
		{
			return html.Replace("{ENCODING}", Encoding.Default.BodyName);
		}

		#endregion
	}
}