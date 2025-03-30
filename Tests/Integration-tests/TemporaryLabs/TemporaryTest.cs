//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Schema;
//using HansKindberg.Text.Formatting.Xml;
//using HtmlAgilityPack;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace IntegrationTests.TemporaryLabs
//{
//	[TestClass]
//	public class TemporaryTest
//	{
//		#region Methods

//		protected internal virtual IEnumerable<XmlNode> GetDescendants(XmlNode xmlNode)
//		{
//			if(xmlNode == null)
//				throw new ArgumentNullException(nameof(xmlNode));

//			foreach(XmlNode childNode in xmlNode.ChildNodes)
//			{
//				yield return childNode;

//				foreach(var grandChildNode in this.GetDescendants(childNode))
//				{
//					yield return grandChildNode;
//				}
//			}
//		}

//		protected internal virtual async Task<string> GetFileContent(string fileName)
//		{
//	        await Task.CompletedTask;
//
//          return File.ReadAllText(Path.Combine(Global.ProjectDirectoryPath, "TemporaryLabs", "Resources", fileName));
//      }

//		protected internal virtual async Task ReverseAttributes(XmlNode xmlNode)
//		{
//			await Task.CompletedTask;

//			if(xmlNode?.Attributes == null)
//				return;

//			var attributes = xmlNode.Attributes.Cast<XmlAttribute>().ToList();
//			xmlNode.Attributes.RemoveAll();

//			attributes.Reverse();

//			foreach(var attribute in attributes)
//			{
//				xmlNode.Attributes.Append(attribute);
//			}
//		}

//		[TestMethod]
//		public async Task Test1()
//		{
//			await Task.CompletedTask;
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		public async Task Test10()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-2.xml");

//			using(var stringReader = new StringReader(fileContent))
//			{
//				using(var xmlReader = XmlReader.Create(stringReader))
//				{
//					var xmlDocument = new XmlDocument();
//					xmlDocument.Load(xmlReader);
//				}
//			}
//		}

//		[TestMethod]
//		public async Task Test2()
//		{
//			await Task.CompletedTask;

//			var fileContent = await this.GetFileContent("Test-1.xml");

//			var htmlDocument = new HtmlDocument();
//			htmlDocument.LoadHtml(fileContent);
//			Assert.AreEqual(3, htmlDocument.DocumentNode.ChildNodes.Count);

//			foreach(var b in htmlDocument.DocumentNode.ChildNodes)
//			{
//				var d = b.NodeType;

//				if(d == null) { }

//				var c = b;

//				if(c == null) { }
//			}
//		}

//		[TestMethod]
//		public async Task Test3()
//		{
//			await Task.CompletedTask;

//			//var fileContent = await this.GetFileContent("Test-3.xml");

//			//var xml = (FormattableXml)await new XmlParser().Parse(fileContent);

//			//var descendants = this.GetDescendants(xml.XmlDocument).ToArray();

//			//Assert.AreEqual(6, descendants.Length);

//			//var concreteAttributes = descendants.SelectMany(descendant => descendant.Attributes != null ? descendant.Attributes.Cast<XmlAttribute>() : Enumerable.Empty<XmlAttribute>()).ToArray();

//			//Assert.AreEqual(8, concreteAttributes.Length);

//			//var attributes = concreteAttributes.Select(concreteAttribute => new XmlAttributeWrapper(null, concreteAttribute)).ToArray();

//			//foreach(var attribute in attributes)
//			//{
//			//	const string xPath = "//@a:id";
//			//	var matches = attribute.Matches(xPath);

//			//	if(matches) { }
//			//	else { }
//			//}
//		}

//		[TestMethod]
//		public async Task Test4()
//		{
//			await Task.CompletedTask;
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		public async Task Test5()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-5.xml");

//			var xmlReaderSettings = new XmlReaderSettings
//			{
//				XmlResolver = new MyXmlResolver(),
//			};

//			xmlReaderSettings.ValidationEventHandler += XmlReaderSettingsOnValidationEventHandler;

//			//var xmlNameTable = new NameTable();
//			//var xmlNamespaceManager = new MyXmlNamespaceManager(xmlNameTable);

//			using(var stringReader = new StringReader(fileContent))
//			{
//				using(var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings)) //, new XmlParserContext(xmlNameTable, xmlNamespaceManager, string.Empty, XmlSpace.None)))
//					//using(var xmlReader = XmlReader.Create(stringReader))
//				{
//					var xmlDocument = new XmlDocument();

//					xmlDocument.NodeChanged += delegate(object sender, XmlNodeChangedEventArgs args) { };

//					xmlDocument.NodeInserting += delegate(object sender, XmlNodeChangedEventArgs args) { };

//					//xmlDocument.XmlResolver

//					xmlDocument.Load(xmlReader);

//					var xmlNode = xmlDocument.FirstChild;

//					var outerXml = xmlNode.OuterXml;

//					//var xmlFragment = new XmlDocument().CreateDocumentFragment();
//					//xmlFragment.InnerXml = fileContent;

//					//Assert.AreEqual(5, xmlFragment.ChildNodes.Count);
//				}
//			}
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		[SuppressMessage("Style", "IDE0010:Add missing cases")]
//		public async Task Test5_2()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-5.xml");

//			var roots = new List<XmlDocument>();
//			var nodeTypes = new List<XmlNodeType>();

//			using(var stringReader = new StringReader(fileContent))
//			{
//				using(var xmlTextReader = new XmlTextReader(stringReader))
//				{
//					xmlTextReader.Namespaces = false;

//					while(xmlTextReader.Read())
//					{
//						nodeTypes.Add(xmlTextReader.NodeType);
//						var root = new XmlDocument();

//						// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault
//						switch(xmlTextReader.NodeType)
//						{
//							case XmlNodeType.Element:
//								using(var subtreeReader = xmlTextReader.ReadSubtree())
//								{
//									root.Load(subtreeReader);
//									roots.Add(root);
//								}

//								break;
//							default:
//								//root.LoadXml(xmlTextReader.ReadOuterXml() + "<root />");
//								//root.RemoveChild(root.LastChild);
//								//roots.Add(root);

//								break;
//						}
//						// ReSharper restore SwitchStatementHandlesSomeKnownEnumValuesWithDefault
//					}
//					//var xmlDocument = new XmlDocument();

//					//xmlDocument.Load(xmlTextReader);

//					//var xmlNode = xmlDocument.FirstChild;

//					//var outerXml = xmlNode.OuterXml;

//					////var xmlFragment = new XmlDocument().CreateDocumentFragment();
//					////xmlFragment.InnerXml = fileContent;

//					////Assert.AreEqual(5, xmlFragment.ChildNodes.Count);
//				}
//			}

//			Assert.AreEqual(1, roots.Count);
//			Assert.AreEqual(3, nodeTypes.Count);
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		[SuppressMessage("Style", "IDE0010:Add missing cases")]
//		public async Task Test5_3()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-2.xml");
//			var nodes = new List<XmlDocument>();

//			using(var textReader = new StringReader(fileContent))
//			{
//				using(var xmlReader = new XmlTextReader(textReader))
//				{
//					xmlReader.Namespaces = false;

//					while(xmlReader.Read())
//					{
//						if(xmlReader.NodeType == XmlNodeType.Element)
//						{
//							using(var subtreeReader = xmlReader.ReadSubtree())
//							{
//								while(subtreeReader.Read())
//								{
//									var node = new XmlDocument();
//									node.AppendChild(node.ReadNode(subtreeReader));
//									nodes.Add(node);
//								}
//							}
//						}
//						else
//						{
//							var node = new XmlDocument();
//							node.AppendChild(node.ReadNode(xmlReader));
//							nodes.Add(node);
//						}
//					}
//				}
//			}
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		[SuppressMessage("Security", "CA5372:Use XmlReader For XPathDocument")]
//		public async Task Test6_1()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-6.xml");

//			var nodes = new List<XmlNode>();
//			var htmlDocument = new HtmlDocument();
//			htmlDocument.LoadHtml(fileContent);

//			Assert.AreEqual(29, htmlDocument.DocumentNode.ChildNodes.Count);

//			foreach(var htmlNode in htmlDocument.DocumentNode.ChildNodes)
//			{
//				var xmlDocument = new XmlDocument();

//				if(htmlNode.NodeType == HtmlNodeType.Text) { }
//				else
//				{
//					var html = $"{htmlNode.OuterHtml}{(htmlNode.NodeType == HtmlNodeType.Comment ? "<_ />" : null)}";

//					if(htmlNode.NodeType == HtmlNodeType.Element)
//					{
//						using(var textReader = new StringReader(html))
//						{
//							using(var xmlReader = new XmlTextReader(textReader))
//							{
//								xmlReader.Namespaces = false;

//								xmlDocument.Load(xmlReader);
//							}
//						}
//					}
//					else
//					{
//						xmlDocument.LoadXml(html);
//					}
//				}

//				var a = xmlDocument.ChildNodes.Count;

//				if(htmlNode.NodeType == HtmlNodeType.Comment)
//					xmlDocument.RemoveChild(xmlDocument.LastChild);

//				nodes.AddRange(xmlDocument.ChildNodes.Cast<XmlNode>());
//				//nodeTypes.Add(node.NodeType);
//			}

//			Assert.AreEqual(29, nodes.Count);

//			//var xmlDocument = new XmlDocument();

//			//xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));
//			//xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));
//			//xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));

//			//var fileContent = await this.GetFileContent("Test-6.xml");

//			//using(var textReader = new StringReader(fileContent))
//			//{
//			//	using(var xmlReader = new XmlTextReader(textReader))
//			//	{
//			//		xmlReader.Namespaces = false;

//			//		var xmlDocument = new XmlDocument();
//			//		xmlDocument.Load(xmlReader);
//			//	}
//			//}

//			//using(var textReader = new StringReader($"<root>{fileContent}</root>"))
//			//{
//			//	var document = new XPathDocument(textReader);
//			//	var navigator = document.CreateNavigator();
//			//	//using(var xmlReader = new  MyXmlReader(new XmlReaderSettings{ConformanceLevel = ConformanceLevel.Fragment}, textReader))
//			//	//{
//			//	//	while(xmlReader.Read())
//			//	//	{

//			//	//	}
//			//	//}
//			//}
//		}

//		[TestMethod]
//		[SuppressMessage("Security", "CA3075:Insecure DTD processing in XML")]
//		[SuppressMessage("Security", "CA5372:Use XmlReader For XPathDocument")]
//		public async Task Test6_2()
//		{
//			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//			var fileContent = await this.GetFileContent("Test-6.xml");

//			var formattableXml = (FormattableXml)await new XmlParser().Parse(fileContent);

//			Assert.AreEqual(29, formattableXml.Nodes.Count);

//			var parentNodes = formattableXml.Nodes.Where(node => node.ParentNode == node.OwnerDocument).ToArray();

//			Assert.AreEqual(29, parentNodes.Length);
//		}

//		protected internal virtual async Task<string> ToXml(XmlDocument xmlDocument)
//		{
//			if(xmlDocument == null)
//				throw new ArgumentNullException(nameof(xmlDocument));

//			await Task.CompletedTask;

//			using(var stringWriter = new StringWriter())
//			{
//				using(var xmlWriter = XmlWriter.Create(stringWriter))
//				{
//					xmlDocument.Save(xmlWriter);

//					return stringWriter.ToString();
//				}
//			}
//		}

//		private void XmlReaderSettingsOnValidationEventHandler(object sender, ValidationEventArgs e)
//		{
//			throw new NotImplementedException();
//		}

//		#endregion
//	}

//	public class MyXmlResolver : XmlResolver
//	{
//		#region Methods

//		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
//		{
//			throw new NotImplementedException("YEAH");
//		}

//		#endregion
//	}

//	[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "<Pending>")]
//	public class MyXmlNamespaceManager : XmlNamespaceManager
//	{
//		#region Constructors

//		public MyXmlNamespaceManager(XmlNameTable nameTable) : base(nameTable) { }

//		#endregion

//		#region Methods

//		public override IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
//		{
//			var result = base.GetNamespacesInScope(scope);

//			return result;
//		}

//		public override string LookupNamespace(string prefix)
//		{
//			return prefix;
//		}

//		public override string LookupPrefix(string uri)
//		{
//			var result = base.LookupPrefix(uri);

//			return result;
//		}

//		#endregion
//	}

//	//public class MyXmlReader : XmlTextReader
//	//{
//	//	#region Constructors

//	//	public MyXmlReader(TextReader textReader) : base()
//	//	{

//	//		this.InternalXmlReader = Create(textReader, settings);
//	//	}

//	//	#endregion
//	//}

//	//public class MyXmlNameTable : NameTable
//	//{
//	//	#region Methods

//	//	public override string Add(string key)
//	//	{
//	//		var result = base.Add(key);

//	//		return result;
//	//	}

//	//	public override string Add(char[] key, int start, int len)
//	//	{
//	//		var result = base.Add(key, start, len);

//	//		return result;
//	//	}

//	//	public override string Get(string value)
//	//	{
//	//		var result = base.Get(value);

//	//		return result;
//	//	}

//	//	public override string Get(char[] key, int start, int len)
//	//	{
//	//		var result = base.Get(key, start, len);

//	//		return result;
//	//	}

//	//	public override string ToString()
//	//	{
//	//		var result = base.ToString();

//	//		return result;
//	//	}

//	//	#endregion
//	//}
//}

