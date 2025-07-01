using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using Shared.Extensions;
using YamlDotNet.Core;

namespace IntegrationTests.Yaml
{
	public class YamlParserTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlParserTest"));

		#endregion

		#region Methods

		[Theory]

		[InlineData("Comments-01", "false,false,0,0,1,0")]
		[InlineData("Comments-02", "false,false,0,0,14,0")]
		[InlineData("Comments-03", "true,false,0,0,1,0")]
		[InlineData("Comments-04", "true,false,0,0,0,0;true,false,0,1,1,0")]
		[InlineData("Comments-05", "true,false,0,0,0,0;true,false,0,1,0,0;true,false,0,1,1,0")]
		[InlineData("Comments-06", "true,false,0,0,0,0;true,false,0,1,0,0;true,false,0,1,12,0")]
		[InlineData("Comments-07", "true,false,0,1,0,0;true,false,0,1,12,0")]
		[InlineData("Comments-08", "true,true,0,0,1,0")]
		[InlineData("Comments-09", "true,true,0,0,2,0")]
		[InlineData("Comments-10", "false,true,0,0,1,0;true,false,0,0,0,0;true,true,0,1,12,0")]
		[InlineData("Documents-01", "true,true,0,0,1,0")]
		[InlineData("Documents-02", "false,true,0,0,1,0;true,true,0,0,0,0;true,false,0,3,0,0;true,true,0,1,0,0;true,true,0,3,0,0;true,true,0,0,3,0;true,false,1,0,0,0")]
		[InlineData("Documents-03", "false,true,0,0,1,0;true,true,0,0,0,0;false,false,0,0,0,4;true,false,0,0,0,0;true,true,0,1,0,0;true,true,0,3,0,0;true,true,0,0,3,0;true,false,1,0,0,0;true,true,0,0,0,0")]
		[InlineData("Documents-Directives-Comments-01", "true,false,2,0,0,0")]
		[InlineData("Documents-Directives-Comments-02", "true,false,2,0,1,0")]
		[InlineData("Documents-Directives-Comments-03", "true,false,2,0,0,0")]
		[InlineData("Documents-Directives-Comments-04", "true,false,2,0,1,0")]
		[InlineData("Documents-Directives-Comments-05", "true,true,7,1,1,0")]
		[InlineData("Documents-Directives-Comments-06", "true,true,0,0,0,0;true,true,7,6,0,0;true,true,0,1,0,0")]
		[InlineData("Empty-01", "")]
		[InlineData("Empty-02", "")]
		[InlineData("Nodes-With-Comments-01", "false,false,0,0,0,3")]
		[InlineData("Nodes-With-Comments-02", "false,false,0,0,0,3;true,false,0,0,0,9")]
		[InlineData("Yaml-01", "false,false,0,0,0,2")]
		[InlineData("Yaml-02", "false,false,0,0,0,4")]
		[InlineData("Yaml-03", "false,false,0,0,0,14")]
		[InlineData("Yaml-04", "false,false,0,0,0,3")]
		[InlineData("Yaml-05", "false,false,0,0,0,7")]
		[InlineData("Yaml-06", "false,false,0,0,0,7")]
		[InlineData("Yaml-07", "false,false,0,0,0,7")]
		[InlineData("Yaml-08", "false,false,0,0,0,6")]
		[InlineData("Yaml-09", "false,false,0,0,0,6")]
		[InlineData("Yaml-10", "false,false,0,0,0,6")]
		[InlineData("Yaml-11", "false,false,0,0,0,6")]
		[InlineData("Yaml-12", "false,false,0,0,0,2;true,false,0,0,0,0;true,true,0,1,0,0;false,false,0,0,0,2")]
		[InlineData("Yaml-14", "false,false,0,0,0,4")]
		[InlineData("Yaml-20", "true,false,2,1,0,41;true,false,0,0,0,48;true,false,0,0,0,24")]
		public async Task CreateStream_RegardingTheStructure_ShouldWorkProperly(string fileName, string expectedResult)
		{
			var value = await GetYaml(fileName);

			var expected = new List<Tuple<bool, bool, int, int, int, int>>();
			foreach(var part in expectedResult.Split(';').Where(item => !string.IsNullOrWhiteSpace(item)))
			{
				var items = part.Split(',').Select(item => item.Trim()).ToArray();
				expected.Add(new Tuple<bool, bool, int, int, int, int>(bool.Parse(items[0]), bool.Parse(items[1]), int.Parse(items[2], null), int.Parse(items[3], null), int.Parse(items[4], null), int.Parse(items[5], null)));
			}

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);
			var stream = await parser.CreateStream(tokens);

			Assert.Equal(expected.Count, stream.Documents.Count);

			for(var i = 0; i < expected.Count; i++)
			{
				var document = stream.Documents[i];
				Assert.Equal(expected[i].Item1, document.Start.Explicit);
				Assert.Equal(expected[i].Item2, document.End.Explicit);
				Assert.Equal(expected[i].Item3, document.Directives.Count);
				Assert.Equal(expected[i].Item4, document.LeadingComments.Count);
				Assert.Equal(expected[i].Item5, document.TrailingComments.Count);
				Assert.Equal(expected[i].Item6, document.Nodes.Count() + document.Nodes.SelectMany(node => node.Descendants()).Count());
			}
		}

		[Theory]
		[InlineData("Comments-01", 1)]
		[InlineData("Comments-02", 1)]
		[InlineData("Comments-03", 1)]
		[InlineData("Comments-04", 2)]
		[InlineData("Comments-05", 3)]
		[InlineData("Comments-06", 3)]
		[InlineData("Comments-07", 2)]
		[InlineData("Comments-08", 1)]
		[InlineData("Comments-09", 1)]
		[InlineData("Comments-10", 3)]
		[InlineData("Documents-01", 1)]
		[InlineData("Documents-02", 7)]
		[InlineData("Documents-03", 9)]
		[InlineData("Documents-Directives-Comments-01", 1)]
		[InlineData("Documents-Directives-Comments-02", 1)]
		[InlineData("Documents-Directives-Comments-03", 1)]
		[InlineData("Documents-Directives-Comments-04", 1)]
		[InlineData("Documents-Directives-Comments-05", 1)]
		[InlineData("Documents-Directives-Comments-06", 3)]
		[InlineData("Empty-01", 0)]
		[InlineData("Empty-02", 0)]
		[InlineData("Nodes-With-Comments-01", 1)]
		[InlineData("Nodes-With-Comments-02", 2)]
		[InlineData("Yaml-01", 1)]
		[InlineData("Yaml-02", 1)]
		[InlineData("Yaml-03", 1)]
		[InlineData("Yaml-04", 1)]
		[InlineData("Yaml-05", 1)]
		[InlineData("Yaml-06", 1)]
		[InlineData("Yaml-07", 1)]
		[InlineData("Yaml-08", 1)]
		[InlineData("Yaml-09", 1)]
		[InlineData("Yaml-10", 1)]
		[InlineData("Yaml-11", 1)]
		[InlineData("Yaml-12", 4)]
		[InlineData("Yaml-14", 1)]
		[InlineData("Yaml-20", 3)]
		public async Task CreateStream_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);
			var stream = await parser.CreateStream(tokens);

			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
		}

		[Theory]
		[InlineData("Invalid-01")]
		public async Task ParseToTokens_IfYamlIsInvalid_ShouldThrowASemanticErrorException(string fileName)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();

			var semanticErrorException = await Assert.ThrowsAsync<SemanticErrorException>(async () => await parser.ParseToTokens(value));

			Assert.NotNull(semanticErrorException);
		}

		[Theory]
		[InlineData("Comments-01", 3)]
		[InlineData("Comments-02", 16)]
		[InlineData("Comments-03", 4)]
		[InlineData("Comments-04", 6)]
		[InlineData("Comments-05", 8)]
		[InlineData("Comments-06", 19)]
		[InlineData("Comments-07", 18)]
		[InlineData("Comments-08", 5)]
		[InlineData("Comments-09", 6)]
		[InlineData("Comments-10", 23)]
		[InlineData("Documents-01", 5)]
		[InlineData("Documents-02", 25)]
		[InlineData("Documents-03", 39)]
		[InlineData("Documents-Directives-Comments-01", 5)]
		[InlineData("Documents-Directives-Comments-02", 6)]
		[InlineData("Documents-Directives-Comments-03", 8)]
		[InlineData("Documents-Directives-Comments-04", 9)]
		[InlineData("Documents-Directives-Comments-05", 22)]
		[InlineData("Documents-Directives-Comments-06", 31)]
		[InlineData("Empty-01", 2)]
		[InlineData("Empty-02", 2)]
		[InlineData("Nodes-With-Comments-01", 16)]
		[InlineData("Nodes-With-Comments-02", 52)]
		[InlineData("Yaml-01", 9)]
		[InlineData("Yaml-02", 14)]
		[InlineData("Yaml-03", 33)]
		[InlineData("Yaml-04", 15)]
		[InlineData("Yaml-05", 10)]
		[InlineData("Yaml-06", 13)]
		[InlineData("Yaml-07", 14)]
		[InlineData("Yaml-08", 12)]
		[InlineData("Yaml-09", 13)]
		[InlineData("Yaml-10", 18)]
		[InlineData("Yaml-11", 19)]
		[InlineData("Yaml-12", 18)]
		[InlineData("Yaml-14", 16)]
		[InlineData("Yaml-20", 348)]
		public async Task ParseToTokens_ShouldWorkProperly(string fileName, int expectedNumberOfTokens)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);

			Assert.Equal(expectedNumberOfTokens, tokens.Count);
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
		[InlineData("Comments-03", "0,1")]
		[InlineData("Comments-04", "0,0;1,1")]
		[InlineData("Comments-05", "0,0;1,0;1,1")]
		[InlineData("Comments-06", "0,0;1,0;1,12")]
		[InlineData("Comments-07", "1,0;1,12")]
		[InlineData("Comments-08", "0,1")]
		[InlineData("Comments-09", "0,2")]
		[InlineData("Comments-10", "0,1;0,0;1,12")]
		public async Task Parse_Comments_ShouldWorkProperly(string fileName, string expectedResult)
		{
			var value = await GetYaml(fileName);

			var expected = new List<KeyValuePair<int, int>>();
			foreach(var pair in expectedResult.Split(';'))
			{
				var numbers = pair.Split(',').Select(item => int.Parse(item.Trim(), null)).ToArray();
				expected.Add(new KeyValuePair<int, int>(numbers[0], numbers[1]));
			}

			var parser = await CreateYamlParser();
			var node = await parser.Parse(value);
			Assert.Equal(expected.Count, node.Documents.Count);

			for(var i = 0; i < node.Documents.Count; i++)
			{
				var document = node.Documents.ElementAt(i);
				Assert.Empty(document.Nodes);
				Assert.Equal(expected[i].Key, document.LeadingComments.Count);
				Assert.Equal(expected[i].Key, document.LeadingComments.Count(comment => comment.Value.Trim().StartsWith("Comment ", StringComparison.Ordinal)));
				Assert.Equal(expected[i].Value, document.TrailingComments.Count);
				Assert.Equal(expected[i].Value, document.TrailingComments.Count(comment => comment.Value.Trim().StartsWith("Comment ", StringComparison.Ordinal)));
			}
		}

		[Theory]
		[InlineData("Documents-Directives-Comments-01", 1, 2, 0)]
		[InlineData("Documents-Directives-Comments-02", 1, 2, 1)]
		[InlineData("Documents-Directives-Comments-03", 1, 2, 0)]
		[InlineData("Documents-Directives-Comments-04", 1, 2, 1)]
		[InlineData("Documents-Directives-Comments-05", 1, 7, 2)]
		[InlineData("Documents-Directives-Comments-06", 3, 7, 7)]
		public async Task Parse_DocumentsDirectivesComments_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments, int expectedNumberOfDirectives, int expectedNumberOfComments)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
			Assert.Equal(expectedNumberOfDirectives, stream.Documents.Sum(document => document.Directives.Count));
			Assert.Equal(expectedNumberOfComments, stream.Documents.Sum(document => document.LeadingComments.Count + document.TrailingComments.Count));
		}

		[Theory]
		[InlineData("Comments-01", "1")]
		[InlineData("Comments-02", "14")]
		[InlineData("Comments-03", "1")]
		[InlineData("Comments-04", "0,2")]
		[InlineData("Comments-05", "0,1,2")]
		[InlineData("Comments-06", "0,1,13")]
		[InlineData("Comments-07", "1,13")]
		[InlineData("Comments-08", "1")]
		[InlineData("Comments-09", "2")]
		[InlineData("Comments-10", "1,0,13")]
		[InlineData("Documents-01", "1")]
		[InlineData("Documents-02", "1,0,3,1,3,3,1")]
		[InlineData("Documents-03", "1,0,14,0,1,3,3,1,0")]
		[InlineData("Documents-Directives-Comments-01", "2")]
		[InlineData("Documents-Directives-Comments-02", "3")]
		[InlineData("Documents-Directives-Comments-03", "4")]
		[InlineData("Documents-Directives-Comments-04", "5")]
		[InlineData("Documents-Directives-Comments-05", "16")]
		[InlineData("Documents-Directives-Comments-06", "0,20,1")]
		[InlineData("Empty-01", "")]
		[InlineData("Empty-02", "")]
		[InlineData("Nodes-With-Comments-01", "13")]
		[InlineData("Nodes-With-Comments-02", "13,35")]
		[InlineData("Yaml-01", "7")]
		[InlineData("Yaml-02", "12")]
		[InlineData("Yaml-03", "31")]
		[InlineData("Yaml-04", "13")]
		[InlineData("Yaml-05", "8")]
		[InlineData("Yaml-06", "11")]
		[InlineData("Yaml-07", "12")]
		[InlineData("Yaml-08", "10")]
		[InlineData("Yaml-09", "11")]
		[InlineData("Yaml-10", "16")]
		[InlineData("Yaml-11", "17")]
		[InlineData("Yaml-12", "6,0,1,6")]
		[InlineData("Yaml-14", "14")]
		[InlineData("Yaml-20", "124,143,76")]
		public async Task CreateDocumentMap_ShouldWorkProperly(string fileName, string expectedResult)
		{
			var value = await GetYaml(fileName);

			var expected = expectedResult.Split(',').Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => int.Parse(item.Trim(), null)).ToList();

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);
			var stream = new YamlStream(tokens.First(), tokens.Last());
			tokens.RemoveAt(0);
			tokens.RemoveAt(tokens.Count - 1);

			var map = await parser.CreateDocumentMap(stream, tokens);

			Assert.Equal(expected.Count, map.Count);

			for(var i = 0; i < expected.Count; i++)
			{
				Assert.Equal(expected[i], map.ElementAt(i).Value.Count);
			}
		}

		[Theory]
		[InlineData("Empty-01")]
		[InlineData("Empty-02")]
		public async Task Parse_Empty_ShouldWorkProperly(string fileName)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);

			Assert.Empty(stream.Documents);
		}



		[Theory]
		[InlineData("Comments-01", 1, 0, 1, 0)]
		[InlineData("Comments-02", 1, 0, 14, 0)]
		[InlineData("Comments-03", 1, 0, 1, 0)]
		[InlineData("Comments-04", 2, 0, 2, 0)]
		[InlineData("Comments-05", 3, 0, 3, 0)]
		[InlineData("Comments-06", 3, 0, 14, 0)]
		[InlineData("Comments-07", 2, 0, 14, 0)]
		[InlineData("Comments-08", 1, 0, 1, 0)]
		[InlineData("Comments-09", 1, 0, 2, 0)]
		[InlineData("Comments-10", 3, 0, 14, 0)]
		[InlineData("Documents-01", 1, 0, 1, 0)]
		[InlineData("Documents-02", 7, 1, 11, 0)]
		[InlineData("Documents-03", 9, 1, 8, 4)]
		[InlineData("Documents-Directives-Comments-01", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-02", 1, 2, 1, 0)]
		[InlineData("Documents-Directives-Comments-03", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-04", 1, 2, 1, 0)]
		[InlineData("Documents-Directives-Comments-05", 1, 7, 2, 0)]
		[InlineData("Documents-Directives-Comments-06", 3, 7, 7, 0)]
		[InlineData("Empty-01", 0, 0, 0, 0)]
		[InlineData("Empty-02", 0, 0, 0, 0)]
		[InlineData("Nodes-With-Comments-01", 1, 0, 0, 3)]
		[InlineData("Nodes-With-Comments-02", 2, 0, 0, 12)]
		[InlineData("Yaml-01", 1, 0, 0, 2)]
		[InlineData("Yaml-02", 1, 0, 0, 4)]
		[InlineData("Yaml-03", 1, 0, 0, 14)]
		[InlineData("Yaml-04", 1, 0, 0, 3)]
		[InlineData("Yaml-05", 1, 0, 0, 7)]
		[InlineData("Yaml-06", 1, 0, 0, 7)]
		[InlineData("Yaml-07", 1, 0, 0, 7)]
		[InlineData("Yaml-08", 1, 0, 0, 6)]
		[InlineData("Yaml-09", 1, 0, 0, 6)]
		[InlineData("Yaml-10", 1, 0, 0, 6)]
		[InlineData("Yaml-11", 1, 0, 0, 6)]
		[InlineData("Yaml-12", 4, 0, 1, 4)]
		[InlineData("Yaml-14", 1, 0, 0, 4)]
		[InlineData("Yaml-20", 3, 2, 1, 113)]
		public async Task Parse_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments, int expectedNumberOfDirectives, int expectedNumberOfComments, int expectedNumberOfDescendants)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
			Assert.Equal(expectedNumberOfDirectives, stream.Documents.Sum(document => document.Directives.Count));
			Assert.Equal(expectedNumberOfComments, stream.Documents.Sum(document => document.LeadingComments.Count + document.TrailingComments.Count));
			Assert.Equal(expectedNumberOfDescendants, stream.Descendants().Count());
		}

		private static async Task ParseNodesWithCommentsShouldWorkProperly(IList<IYamlDocument> documents)
		{
			await Task.CompletedTask;

			var firstDocument = documents[0];
			Assert.Single(firstDocument.Nodes);
			Assert.Empty(firstDocument.LeadingComments);
			Assert.Empty(firstDocument.TrailingComments);

			var firstNode = firstDocument.Nodes.ElementAt(0);
			Assert.Equal(2, firstNode.Children.Count());
			Assert.Empty(firstNode.LeadingComments);
			Assert.Empty(firstNode.TrailingComments);

			var firstChild = (IYamlContentNode)firstNode.Children.ElementAt(0);
			Assert.Equal(2, firstChild.LeadingComments.Count);
			Assert.Equal("Comment 01", firstChild.LeadingComments[0].Value);
			Assert.Equal("Comment 02", firstChild.LeadingComments[1].Value);
			Assert.Empty(firstChild.TrailingComments);
			Assert.NotNull(firstChild.Anchor);
			Assert.Equal("b", firstChild.Anchor.Value);
			Assert.Null(firstChild.AnchorAlias);
			Assert.NotNull(firstChild.Comment);
			Assert.True(firstChild.Comment.IsInline);
			Assert.Equal("Comment 03", firstChild.Comment.Value);
			Assert.NotNull(firstChild.Key);
			Assert.Equal(ScalarStyle.Plain, firstChild.Key.Style);
			Assert.True(firstChild.Key.IsKey);
			Assert.Equal("a", firstChild.Key.Value);
			Assert.Null(firstChild.Tag);
			Assert.Null(firstChild.Value);

			var secondChild = (IYamlContentNode)firstNode.Children.ElementAt(1);
			Assert.Empty(secondChild.LeadingComments);
			Assert.Single(secondChild.TrailingComments);
			Assert.Equal("Comment 04", secondChild.TrailingComments[0].Value);
			Assert.Null(secondChild.Anchor);
			Assert.NotNull(secondChild.AnchorAlias);
			Assert.Equal("b", secondChild.AnchorAlias.Value);
			Assert.Null(secondChild.Comment);
			Assert.NotNull(secondChild.Key);
			Assert.Equal(ScalarStyle.Plain, secondChild.Key.Style);
			Assert.True(secondChild.Key.IsKey);
			Assert.Equal("c", secondChild.Key.Value);
			Assert.Null(secondChild.Tag);
			Assert.Null(secondChild.Value);
		}

		[Fact]
		public async Task Parse_NodesWithComments_ShouldWorkProperly()
		{
			var value = await GetYaml("Nodes-With-Comments-01");
			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			var documents = stream.Documents;
			Assert.Single(documents);
			await ParseNodesWithCommentsShouldWorkProperly(documents);

			value = await GetYaml("Nodes-With-Comments-02");
			parser = await CreateYamlParser();
			stream = await parser.Parse(value);
			documents = stream.Documents;
			Assert.Equal(2, documents.Count);
			await ParseNodesWithCommentsShouldWorkProperly(documents);
			var secondDocument = documents[1];
			Assert.Single(secondDocument.Nodes);
			Assert.Empty(secondDocument.LeadingComments);
			Assert.Empty(secondDocument.TrailingComments);

			var firstNode = secondDocument.Nodes.ElementAt(0);
			Assert.Equal(2, firstNode.Children.Count());
			Assert.Empty(firstNode.LeadingComments);
			Assert.Empty(firstNode.TrailingComments);

















			//Assert.Single(documents[1].LeadingComments);
			//Assert.Equal("Comment 3", documents[1].LeadingComments[0].Value);
			//Assert.Equal(2, documents[1].Children.Count());
			//Assert.Single(documents[1].Children.ElementAt(0).LeadingComments);
			//Assert.Equal("Comment 4", documents[1].Children.ElementAt(0).LeadingComments[0].Value);
			//Assert.Equal(3, documents[1].Children.ElementAt(0).Children.Count());
			//Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments);
			//Assert.Equal("Comment 5", documents[1].Children.ElementAt(0).Children.ElementAt(0).LeadingComments[0].Value);
			//Assert.Equal("Comment 6", documents[1].Children.ElementAt(0).Children.ElementAt(1).Comment!.Value);
			//Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(2).Children);
			//Assert.Single(documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments);
			//Assert.Equal("Comment 7", documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).LeadingComments[0].Value);
			//Assert.Equal("Comment 8", documents[1].Children.ElementAt(0).Children.ElementAt(2).Children.ElementAt(0).Comment!.Value);
			//Assert.Single(documents[1].Children.ElementAt(1).LeadingComments);
			//Assert.Equal("Comment 9", documents[1].Children.ElementAt(1).LeadingComments[0].Value);
			//Assert.Single(documents[1].TrailingComments);
			//Assert.Equal("Comment 10", documents[1].TrailingComments[0].Value);
			//Assert.Empty(documents[1].Children.ElementAt(1).Children);
		}

		#endregion













		[Fact]
		public async Task Arne()
		{
			var value = await GetYaml("_arne");
			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			Assert.NotNull(stream);
			Assert.Single(stream.Documents);
			var document = stream.Documents[0];
			Assert.Single(document.Nodes);
			var node = document.Nodes.First();
			Assert.Equal(3, node.Children.Count());
		}
	}
}