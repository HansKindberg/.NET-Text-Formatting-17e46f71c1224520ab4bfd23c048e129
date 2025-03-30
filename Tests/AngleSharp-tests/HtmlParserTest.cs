using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace AngleSharpTests
{
	public class HtmlParserTest
	{
		#region Methods

		[Theory]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\" />", "<html><head><link href=\"/Style/Site.css\" rel=\"stylesheet\"></head><body></body></html>")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\"></link>", "<html><head><link href=\"/Style/Site.css\" rel=\"stylesheet\"></head><body></body></html>")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\">", "<html><head><link href=\"/Style/Site.css\" rel=\"stylesheet\"></head><body></body></html>")]
		public async Task ParseDocument_LinkElement_Test(string html, string expectedOuterHtml)
		{
			await Task.CompletedTask;

			var htmlParser = new HtmlParser();
			var htmlDocument = htmlParser.ParseDocument(html);

			Assert.Equal(expectedOuterHtml, htmlDocument.DocumentElement.OuterHtml);
		}

		[Theory]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\" />", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\"></link>", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">")]
		[InlineData("<link href=\"/Style/Site.css\" rel=\"stylesheet\">", "<link href=\"/Style/Site.css\" rel=\"stylesheet\">")]
		[InlineData("<link />", "<link>")]
		[InlineData("<link></link>", "<link>")]
		[InlineData("<link>", "<link>")]
		[InlineData("<p />", "<p></p>")]
		[InlineData("<p></p>", "<p></p>")]
		[InlineData("<p>", "<p></p>")]
		public async Task ParseFragment_LinkElement_Test(string html, string expectedOuterHtml)
		{
			await Task.CompletedTask;

			var options = new HtmlParserOptions
			{
				DisableElementPositionTracking = true,
				IsAcceptingCustomElementsEverywhere = true
			};

			var htmlParser = new HtmlParser(options);
			var nodeList = htmlParser.ParseFragment(html, new HtmlElement(null!, string.Empty));

			Assert.Single(nodeList);
			var node = nodeList[0];
			Assert.NotNull(node);
			var element = node as IElement;
			Assert.NotNull(element);
			Assert.Equal(expectedOuterHtml, element.OuterHtml);
		}

		#endregion
	}
}