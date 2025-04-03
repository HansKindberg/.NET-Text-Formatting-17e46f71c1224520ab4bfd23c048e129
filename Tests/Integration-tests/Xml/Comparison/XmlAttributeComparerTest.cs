using System.Xml;
using HansKindberg.Text.Formatting.Xml;
using HansKindberg.Text.Formatting.Xml.Comparison;

namespace IntegrationTests.Xml.Comparison
{
	public class XmlAttributeComparerTest
	{
		#region Methods

		[Fact]
		public async Task Compare_Pinned_ShouldSortAListCorrectly()
		{
			var options = new XmlAttributeFormat();

			options.Pinned.Add("//@pinned");
			var attributes = await this.GetSortedAttributes(options);
			Assert.Equal("pinned", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("@pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("pinned", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("a", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("//element/@pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("pinned", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("/element/@pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("a", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("/root/element/@pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("pinned", attributes[0].Name);

			options.Pinned.Clear();
			options.Pinned.Add("@pinned");
			options.Pinned.Add("@x");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("pinned", attributes[0].Name);
			Assert.Equal("x", attributes[1].Name);

			options.Pinned.Clear();
			options.Pinned.Add("@x");
			options.Pinned.Add("@pinned");
			attributes = await this.GetSortedAttributes(options);
			Assert.Equal("x", attributes[0].Name);
			Assert.Equal("pinned", attributes[1].Name);
		}

		[Fact]
		public async Task Compare_ShouldSortAListCorrectly()
		{
			var attributes = await this.GetSortedAttributes(new XmlAttributeFormat());

			Assert.Equal(8, attributes.Count);

			var attribute = attributes[0];
			Assert.Equal("a", attribute.Name);
			Assert.Equal("a", attribute.Value);
			Assert.Equal(6, attribute.InitialIndex);
			Assert.Equal("a", attribute.XmlNode.Name);
			Assert.Equal("a", attribute.XmlNode.Value);

			attribute = attributes[1];
			Assert.Equal("a:a", attribute.Name);
			Assert.Equal("a:a", attribute.Value);
			Assert.Equal(3, attribute.InitialIndex);
			Assert.Equal("a:a", attribute.XmlNode.Name);
			Assert.Equal("a:a", attribute.XmlNode.Value);

			attribute = attributes[2];
			Assert.Equal("b", attribute.Name);
			Assert.Equal("b", attribute.Value);
			Assert.Equal(5, attribute.InitialIndex);
			Assert.Equal("b", attribute.XmlNode.Name);
			Assert.Equal("b", attribute.XmlNode.Value);

			attribute = attributes[3];
			Assert.Equal("b:a", attribute.Name);
			Assert.Equal("b:a", attribute.Value);
			Assert.Equal(2, attribute.InitialIndex);
			Assert.Equal("b:a", attribute.XmlNode.Name);
			Assert.Equal("b:a", attribute.XmlNode.Value);

			attribute = attributes[4];
			Assert.Equal("c", attribute.Name);
			Assert.Equal("c", attribute.Value);
			Assert.Equal(4, attribute.InitialIndex);
			Assert.Equal("c", attribute.XmlNode.Name);
			Assert.Equal("c", attribute.XmlNode.Value);

			attribute = attributes[5];
			Assert.Equal("c:a", attribute.Name);
			Assert.Equal("c:a", attribute.Value);
			Assert.Equal(1, attribute.InitialIndex);
			Assert.Equal("c:a", attribute.XmlNode.Name);
			Assert.Equal("c:a", attribute.XmlNode.Value);

			attribute = attributes[6];
			Assert.Equal("pinned", attribute.Name);
			Assert.Equal("pinned", attribute.Value);
			Assert.Equal(7, attribute.InitialIndex);
			Assert.Equal("pinned", attribute.XmlNode.Name);
			Assert.Equal("pinned", attribute.XmlNode.Value);

			attribute = attributes[7];
			Assert.Equal("x", attribute.Name);
			Assert.Equal("&amp;", attribute.Value);
			Assert.Equal(0, attribute.InitialIndex);
			Assert.Equal("x", attribute.XmlNode.Name);
			Assert.Equal("&", attribute.XmlNode.Value);
		}

		protected internal virtual async Task<string> GetFileContent(string fileName)
		{
			await Task.CompletedTask;

			return File.ReadAllText(Path.Combine(Global.ProjectDirectory.FullName, "Xml", "Comparison", "Resources", fileName));
		}

		protected internal virtual async Task<IList<XmlAttributeWrapper>> GetSortedAttributes(IXmlAttributeFormat options)
		{
			var xmlAttributeCollection = await this.GetXmlAttributeCollection();

			var attributes = xmlAttributeCollection.Cast<XmlAttribute>().Select((xmlAttribute, index) => new XmlAttributeWrapper(null, xmlAttribute) { InitialIndex = index }).ToList();

			attributes.Sort(new XmlAttributeComparer(options));

			return attributes;
		}

		protected internal virtual async Task<XmlAttributeCollection> GetXmlAttributeCollection()
		{
			var fileContent = await this.GetFileContent("XmlAttributeComparerTest.xml");

			using(var stringReader = new StringReader(fileContent))
			{
				using(var xmlReader = XmlReader.Create(stringReader))
				{
					var xmlDocument = new XmlDocument();
					xmlDocument.Load(xmlReader);

					return xmlDocument.FirstChild!.FirstChild!.Attributes!;
				}
			}
		}

		#endregion
	}
}