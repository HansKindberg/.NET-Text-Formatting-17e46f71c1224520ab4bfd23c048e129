using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using Shared.Extensions;

namespace IntegrationTests.Yaml
{
	public class NewYamlParserTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlParserTest"));

		#endregion

		#region Methods

		[Theory]
		[InlineData("Comments-01", 3)]
		[InlineData("Comments-02", 16)]
		[InlineData("Comments-03", 19)]
		[InlineData("Comments-04", 18)]
		[InlineData("Comments-05", 23)]
		[InlineData("Documents-Directives-Comments-01", 22)]
		[InlineData("Documents-Directives-Comments-02", 26)]
		[InlineData("Empty-01", 2)]
		[InlineData("Empty-02", 2)]
		[InlineData("Nodes-With-Comments-01", 15)]
		[InlineData("Nodes-With-Comments-02", 51)]
		[InlineData("Yaml-01", 9)]
		[InlineData("Yaml-02", 14)]
		[InlineData("Yaml-03", 33)]
		[InlineData("Yaml-04", 15)]
		[InlineData("Yaml-05", 10)]
		[InlineData("Yaml-06", 12)]
		[InlineData("Yaml-07", 13)]
		[InlineData("Yaml-08", 12)]
		[InlineData("Yaml-09", 13)]
		[InlineData("Yaml-10", 18)]
		[InlineData("Yaml-11", 19)]
		[InlineData("Yaml-12", 18)]
		[InlineData("Yaml-20", 347)]
		public async Task CreateTokens_ShouldWorkProperly(string fileName, int expectedNumberOfItems)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var tokens = await yamlParser.CreateTokens(value);

			Assert.Equal(expectedNumberOfItems, tokens.Count);
		}

		private static async Task<NewYamlParser> CreateYamlParser()
		{
			await Task.CompletedTask;

			return new NewYamlParser();
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
		[InlineData("Comments-03", "0,0;1,0;1,12")]
		[InlineData("Comments-04", "0,0;1,0;1,12")]
		[InlineData("Comments-05", "0,0;1,0;1,12")]
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
			var node = await yamlParser.Parse(value);
			Assert.Equal(expected.Count, node.Children.Count());

			for(var i = 0; i < node.Children.Count(); i++)
			{
				var child = node.Children.ElementAt(i);
				Assert.Empty(child.Children);
				Assert.Null(child.Comment);
				Assert.Equal(expected[i].Key, child.LeadingComments.Count);
				Assert.Equal(expected[i].Key, child.LeadingComments.Count(comment => comment.Value.Trim() == "Comment"));
				Assert.Equal(expected[i].Value, child.TrailingComments.Count);
				Assert.Equal(expected[i].Value, child.TrailingComments.Count(comment => comment.Value.Trim() == "Comment"));
			}
		}

		[Theory]
		[InlineData("Documents-Directives-Comments-01")]
		[InlineData("Documents-Directives-Comments-02")]
		public async Task Parse_DocumentsDirectivesComments_ShouldWorkProperly(string fileName)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var node = await yamlParser.Parse(value);
			Assert.NotNull(node);
		}

		[Theory]
		[InlineData("Empty-01")]
		[InlineData("Empty-02")]
		public async Task Parse_Empty_ShouldWorkProperly(string fileName)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var node = await yamlParser.Parse(value);

			Assert.Empty(node.Children);
		}

		[Fact]
		public async Task Parse_NodesWithComments_ShouldWorkProperly()
		{
			var value = await GetYaml("Nodes-With-Comments-01");
			var yamlParser = await CreateYamlParser();
			var node = await yamlParser.Parse(value);
			var documents = node.Children.ToList();
			Assert.Single(documents);
			await ParseNodesWithCommentsShouldWorkProperly(documents);
			Assert.Single(documents[0].TrailingComments);
			Assert.Equal("Comment 3", documents[0].TrailingComments[0].Value);

			value = await GetYaml("Nodes-With-Comments-02");
			yamlParser = await CreateYamlParser();
			node = await yamlParser.Parse(value);
			documents = [.. node.Children];
			Assert.Equal(2, documents.Count);
			await ParseNodesWithCommentsShouldWorkProperly(documents);
			Assert.Single(documents[1].LeadingComments);
			Assert.Equal("Comment 3", documents[1].LeadingComments[0].Value);
			Assert.Equal(2, documents[1].Children.Count());
			Assert.Single(documents[1].Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 4", documents[1].Children.ElementAt(0).LeadingComments[0].Value);
			Assert.Equal(3, documents[1].Children.ElementAt(0).Children.Count());
			Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 5", documents[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments[0].Value);
			Assert.Equal("Comment 6", documents[1].Children.ElementAt(0).Children.ElementAt(1).Comment!.Value);
			Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(2).Children);
			Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments);
			Assert.Equal("Comment 7", documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments[0].Value);
			Assert.Equal("Comment 8", documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).Comment!.Value);
			Assert.Single(documents[1].Children.ElementAt(1).LeadingComments);
			Assert.Equal("Comment 9", documents[1].Children.ElementAt(1).LeadingComments[0].Value);
			Assert.Single(documents[1].TrailingComments);
			Assert.Equal("Comment 10", documents[1].TrailingComments[0].Value);
			Assert.Empty(documents[1].Children.ElementAt(1).Children);
		}

		[Theory]
		[InlineData("Yaml-01", 2)]
		[InlineData("Yaml-02", 3)]
		[InlineData("Yaml-03", 9)]
		[InlineData("Yaml-04", 3)]
		[InlineData("Yaml-05", 4)]
		[InlineData("Yaml-06", 4)]
		[InlineData("Yaml-07", 4)]
		[InlineData("Yaml-08", 4)]
		[InlineData("Yaml-09", 4)]
		[InlineData("Yaml-10", 4)]
		[InlineData("Yaml-11", 4)]
		[InlineData("Yaml-12", 6)]
		[InlineData("Yaml-20", 80)]
		public async Task Parse_ShouldWorkProperly(string fileName, int expectedNumberOfNodes)
		{
			var value = await GetYaml(fileName);

			var yamlParser = await CreateYamlParser();
			var node = await yamlParser.Parse(value);
			var numberOfNodes = node.Descendants().Count();

			Assert.Equal(expectedNumberOfNodes, numberOfNodes);
		}

		private static async Task ParseNodesWithCommentsShouldWorkProperly(List<IYamlNode> nodes)
		{
			await Task.CompletedTask;

			Assert.Equal(2, nodes[0].Children.Count());
			Assert.Equal(2, nodes[0].LeadingComments.Count);
			Assert.Equal("Comment 1", nodes[0].LeadingComments[0].Value);
			Assert.Equal("Comment 2", nodes[0].LeadingComments[1].Value);
		}

		#endregion
	}
}