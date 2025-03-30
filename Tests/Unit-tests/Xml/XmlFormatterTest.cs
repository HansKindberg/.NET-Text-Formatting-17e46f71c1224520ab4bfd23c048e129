using HansKindberg.Text.Formatting;
using HansKindberg.Text.Formatting.Xml;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Xml
{
	public class XmlFormatterTest
	{
		#region Methods

		protected internal virtual async Task<XmlFormatter> CreateXmlFormatter(IParser<IXmlFragment>? parser = null)
		{
			await Task.CompletedTask;

			return new XmlFormatter(Mock.Of<ILoggerFactory>(), parser ?? Mock.Of<IParser<IXmlFragment>>());
		}

		[Fact]
		public async Task Format_IfTheOptionsParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await (await this.CreateXmlFormatter()).Format(null, "<root />"));
			Assert.Equal("options", argumentNullException.ParamName);
		}

		[Fact]
		public async Task Format_IfTheXmlParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await (await this.CreateXmlFormatter()).Format(new XmlFormat(), null));
			Assert.Equal("text", argumentNullException.ParamName);
		}

		#endregion

		//[TestMethod]
		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA3075:Insecure DTD processing in XML", Justification = "<Pending>")]
		//public async Task Format_Test()
		//{
		//	await Task.CompletedTask;

		//	CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
		//	CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

		//	const string xml = "<root xmlns:yy=\"http://localhost\" xmlns:b=\"http://localhost\" yy=\"\" yy:arne=\"value\" b=\"\" b:yeah=\"a-value\" arne=\"terpa\"><a b=\"b\" a=\"a\" /></root>";

		//	//using(var stringReader = new StringReader(xml))
		//	//{
		//	//	using(var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings{ConformanceLevel = ConformanceLevel.Auto, DtdProcessing = DtdProcessing.Ignore}))
		//	//}
		//	var xDocument = XDocument.Parse(xml);

		//	var a = xDocument.Root.Elements().ElementAt(0).Attributes().First();
		//	if(a == null) { }

		//	a.Parent.XPathEvaluate()

		//	var xmlDocument = new XmlDocument();
		//	xmlDocument.LoadXml(xml);
		//	var b = xmlDocument.DocumentElement.ChildNodes[0].Attributes[0];
		//	if(b == null) { }

		//	b.ParentNode.XP

		//	//Assert.AreEqual("<root />", new XmlFormatter().Format("<root />", new XmlFormat()));
		//}

		//protected internal virtual async Task<string> GetXml(string fileName)
		//{
		//	await Task.CompletedTask;

		//	var path = Path.Combine(Global.ProjectDirectoryPath, @"Xml\Resources", $"{fileName}.xml");

		//	return File.ReadAllText(path);
		//}

		//[TestMethod]
		//public async Task Parse_IfTheXmlParameterContainsOnlyWhitespaces_ShouldParseCorrectly()
		//{
		//	await Task.CompletedTask;

		//	var xmlDocument = new XmlFormatter().Parse("   ");

		//	Assert.IsNotNull(xmlDocument);
		//	Assert.AreEqual(0, xmlDocument.ChildNodes.Count);
		//	Assert.IsNull(xmlDocument.DocumentElement);
		//}

		//[TestMethod]
		//public async Task Parse_IfTheXmlParameterIsAnEmptyString_ShouldParseCorrectly()
		//{
		//	await Task.CompletedTask;

		//	var xmlDocument = new XmlFormatter().Parse(string.Empty);

		//	Assert.IsNotNull(xmlDocument);
		//	Assert.AreEqual(0, xmlDocument.ChildNodes.Count);
		//	Assert.IsNull(xmlDocument.DocumentElement);
		//}

		//[TestMethod]
		//public async Task Parse_IfTheXmlParameterIsAnXmlDeclarationOnly_ShouldParseCorrectly()
		//{
		//	await Task.CompletedTask;

		//	var xmlDocument = new XmlFormatter().Parse("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

		//	Assert.IsNotNull(xmlDocument);
		//	Assert.AreEqual(1, xmlDocument.ChildNodes.Count);
		//	Assert.IsTrue(xmlDocument.ChildNodes[0].NodeType == XmlNodeType.XmlDeclaration);
		//	Assert.IsTrue(xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration);
		//	Assert.IsNull(xmlDocument.DocumentElement);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public async Task Parse_IfTheXmlParameterIsNull_ShouldThrowAnInvalidOperationException()
		//{
		//	await Task.CompletedTask;

		//	try
		//	{
		//		new XmlFormatter().Parse(null);
		//	}
		//	catch(InvalidOperationException invalidOperationException)
		//	{
		//		if(string.Equals(invalidOperationException.Message, "Could not parse xml null.", StringComparison.Ordinal))
		//			throw;
		//	}
		//}
	}
}