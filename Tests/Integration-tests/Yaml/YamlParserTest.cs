using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using Shared.Extensions;

namespace IntegrationTests.Yaml
{
	public class YamlParserTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlParserTest"));

		#endregion

		#region Methods

		[Theory]
		[InlineData("Yaml-01", 9)]
		[InlineData("Yaml-02", 12)]
		[InlineData("Yaml-03", 24)]
		[InlineData("Yaml-04", 13)]
		[InlineData("Yaml-05", 9)]
		public async Task CreateParsingEvents_ShouldWorkProperly(string fileName, int expectedNumberOfItems)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var parsingEvents = await yamlParser.CreateParsingEvents(value);

			Assert.Equal(expectedNumberOfItems, parsingEvents.Count);
		}

		private static async Task<YamlParser> CreateYamlParser()
		{
			await Task.CompletedTask;

			return new YamlParser();
		}

		private static async Task<string> GetFileText(string path)
		{
			await Task.CompletedTask;

			// ReSharper disable MethodHasAsyncOverload
			return File.ReadAllText(path).ResolveNewLine();
			// ReSharper restore MethodHasAsyncOverload
		}

		private static async Task<string> GetYaml(string fileName)
		{
			return await GetFileText(Path.Combine(_resourcesDirectory.FullName, $"{fileName}.yml"));
		}

		[Theory]
		[InlineData("Comments-01", "0,1")]
		[InlineData("Comments-02", "0,14")]
		[InlineData("Comments-03", "0,1;0,1;0,12")]
		public async Task Parse_Comments_ShouldWorkProperly(string fileName, string expectedResult)
		{
			var value = await GetYaml(fileName);

			var expected = new List<KeyValuePair<int, int>>();
			foreach(var pair in expectedResult.Split(';'))
			{
				var numbers = pair.Split(',').Select(item => int.Parse(item.Trim(), null)).ToArray();
				expected.Add(new KeyValuePair<int, int>(numbers[0], numbers[1]));
			}

			var yamlParser = await CreateYamlParser();
			var nodes = await yamlParser.Parse(value);
			Assert.Equal(expected.Count, nodes.Count);

			for(var i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				Assert.Empty(node.Children);
				Assert.Null(node.Comment);
				Assert.Equal(expected[i].Key, node.LeadingComments.Count);
				Assert.Equal(expected[i].Key, node.LeadingComments.Count(comment => comment == "Comment"));
				Assert.Equal(expected[i].Value, node.TrailingComments.Count);
				Assert.Equal(expected[i].Value, node.TrailingComments.Count(comment => comment == "Comment"));
			}
		}

		[Theory]
		[InlineData("Empty-01")]
		[InlineData("Empty-02")]
		public async Task Parse_Empty_ShouldWorkProperly(string fileName)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var nodes = await yamlParser.Parse(value);

			Assert.Empty(nodes);
		}

		[Fact]
		public async Task Parse_NodesWithComments_ShouldWorkProperly()
		{
			var value = await GetYaml("Nodes-With-Comments-01");
			var yamlParser = await CreateYamlParser();
			var nodes = await yamlParser.Parse(value);
			Assert.Single(nodes);
			await ParseNodesWithCommentsShouldWorkProperly(nodes);

			value = await GetYaml("Nodes-With-Comments-02");
			yamlParser = await CreateYamlParser();
			nodes = await yamlParser.Parse(value);
			Assert.Equal(2, nodes.Count);
			await ParseNodesWithCommentsShouldWorkProperly(nodes);
			Assert.Equal(2, nodes[1].Children.Count());
			Assert.Single(nodes[1].Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 4", nodes[1].Children.ElementAt(0).LeadingComments[0]);
			Assert.Equal(3, nodes[1].Children.ElementAt(0).Children.Count());
			Assert.Single(nodes[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 5", nodes[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments[0]);
			Assert.Equal("Comment 6", nodes[1].Children.ElementAt(0).Children.ElementAt(1).Comment);
			Assert.Single(nodes[1].Children.ElementAt(0).Children.ElementAt(2).Children);
			Assert.Single(nodes[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 7", nodes[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments[0]);
			Assert.Equal("Comment 8", nodes[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).Comment);
			Assert.Single(nodes[1].Children.ElementAt(1).LeadingComments);
			Assert.Equal("Comment 9", nodes[1].Children.ElementAt(1).LeadingComments[0]);
			Assert.Single(nodes[1].Children.ElementAt(1).TrailingComments);
			Assert.Equal("Comment 10", nodes[1].Children.ElementAt(1).TrailingComments[0]);
			Assert.Empty(nodes[1].Children.ElementAt(1).Children);
		}

		[Theory]
		[InlineData("Yaml-01", 2)]
		[InlineData("Yaml-02", 3)]
		[InlineData("Yaml-03", 9)]
		[InlineData("Yaml-04", 3)]
		[InlineData("Yaml-05", 4)]
		public async Task Parse_ShouldWorkProperly(string fileName, int expectedNumberOfNodes)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var nodes = await yamlParser.Parse(value);
			var numberOfNodes = nodes.Count + nodes.Sum(node => node.Descendants().Count());

			Assert.Equal(expectedNumberOfNodes, numberOfNodes);
		}

		private static async Task ParseNodesWithCommentsShouldWorkProperly(IList<IYamlNode> nodes)
		{
			await Task.CompletedTask;

			Assert.Equal(2, nodes[0].Children.Count());
			Assert.Equal(2, nodes[0].Children.ElementAt(0).LeadingComments.Count);
			Assert.Equal("Comment 1", nodes[0].Children.ElementAt(0).LeadingComments[0]);
			Assert.Equal("Comment 2", nodes[0].Children.ElementAt(0).LeadingComments[1]);
			Assert.Empty(nodes[0].Children.ElementAt(0).TrailingComments);
			Assert.Empty(nodes[0].Children.ElementAt(1).LeadingComments);
			Assert.Single(nodes[0].Children.ElementAt(1).TrailingComments);
			Assert.Equal("Comment 3", nodes[0].Children.ElementAt(1).TrailingComments[0]);
		}

		[Theory]
		[InlineData("Yaml-01", 1)]
		[InlineData("Yaml-02", 1)]
		[InlineData("Yaml-03", 1)]
		[InlineData("Yaml-04", 1)]
		[InlineData("Yaml-05", 1)]
		public async Task SplitIntoDocuments_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var parsingEvents = await yamlParser.CreateParsingEvents(value);
			var documents = await yamlParser.SplitIntoDocuments(parsingEvents);

			Assert.Equal(expectedNumberOfDocuments, documents.Count);
		}

		#endregion
	}
}