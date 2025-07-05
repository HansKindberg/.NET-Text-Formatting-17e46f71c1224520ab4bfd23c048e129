using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using Shared.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace IntegrationTests.Yaml
{
	public class YamlParserTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlParserTest"));

		#endregion

		#region Methods

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
		[InlineData("Documents-02", "5")]
		[InlineData("Documents-03", "1,0,3,1,3,3,1")]
		[InlineData("Documents-04", "1,0,14,0,1,3,3,1,0")]
		[InlineData("Documents-Directives-Comments-01", "2")]
		[InlineData("Documents-Directives-Comments-02", "3")]
		[InlineData("Documents-Directives-Comments-03", "4")]
		[InlineData("Documents-Directives-Comments-04", "5")]
		[InlineData("Documents-Directives-Comments-05", "16")]
		[InlineData("Documents-Directives-Comments-06", "0,20,1")]
		[InlineData("Empty-01", "")]
		[InlineData("Empty-02", "")]
		[InlineData("Nodes-With-Comments-01", "14")]
		[InlineData("Nodes-With-Comments-02", "14,35")]
		[InlineData("Yaml-01", "7")]
		[InlineData("Yaml-02", "12")]
		[InlineData("Yaml-03", "31")]
		[InlineData("Yaml-04", "13")]
		[InlineData("Yaml-05", "8")]
		[InlineData("Yaml-06", "11")]
		[InlineData("Yaml-07", "12")]
		[InlineData("Yaml-08", "9")]
		[InlineData("Yaml-09", "10")]
		[InlineData("Yaml-10", "15")]
		[InlineData("Yaml-11", "16")]
		[InlineData("Yaml-12", "6,0,1,6")]
		[InlineData("Yaml-14", "14")]
		[InlineData("Yaml-15", "126")]
		[InlineData("Yaml-16", "26")]
		[InlineData("Yaml-17", "10")]
		[InlineData("Yaml-18", "27")]
		[InlineData("Yaml-19", "13")]
		[InlineData("Yaml-50", "124,143,76")]
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
		[InlineData("Comments-01", "false,false,0,0,0,1")]
		[InlineData("Comments-02", "false,false,0,0,0,14")]
		[InlineData("Comments-03", "true,false,0,0,0,1")]
		[InlineData("Comments-04", "true,false,0,0,0,0;true,false,0,1,0,1")]
		[InlineData("Comments-05", "true,false,0,0,0,0;true,false,0,1,0,0;true,false,0,1,0,1")]
		[InlineData("Comments-06", "true,false,0,0,0,0;true,false,0,1,0,0;true,false,0,1,0,12")]
		[InlineData("Comments-07", "true,false,0,1,0,0;true,false,0,1,0,12")]
		[InlineData("Comments-08", "true,true,0,0,0,1")]
		[InlineData("Comments-09", "true,true,0,0,1,1")]
		[InlineData("Comments-10", "false,true,0,0,0,1;true,false,0,0,0,0;true,true,0,1,0,12")]
		[InlineData("Documents-01", "true,true,0,0,1,0")]
		[InlineData("Documents-02", "true,true,0,1,1,3")]
		[InlineData("Documents-03", "false,true,0,0,0,1;true,true,0,0,0,0;true,false,0,3,0,0;true,true,0,1,0,0;true,true,0,3,0,0;true,true,0,0,0,3;true,false,1,0,0,0")]
		[InlineData("Documents-04", "false,true,0,0,0,1;true,true,0,0,0,0;false,false,0,0,0,3;true,false,0,0,0,0;true,true,0,1,0,0;true,true,0,3,0,0;true,true,0,0,0,3;true,false,1,0,0,0;true,true,0,0,0,0")]
		[InlineData("Documents-Directives-Comments-01", "true,false,2,0,0,0")]
		[InlineData("Documents-Directives-Comments-02", "true,false,2,0,0,1")]
		[InlineData("Documents-Directives-Comments-03", "true,false,2,0,0,0")]
		[InlineData("Documents-Directives-Comments-04", "true,false,2,0,0,1")]
		[InlineData("Documents-Directives-Comments-05", "true,true,7,1,1,0")]
		[InlineData("Documents-Directives-Comments-06", "true,true,0,0,0,0;true,true,7,6,0,0;true,true,0,1,0,0")]
		[InlineData("Empty-01", "")]
		[InlineData("Empty-02", "")]
		[InlineData("Nodes-With-Comments-01", "false,false,0,0,0,2")]
		[InlineData("Nodes-With-Comments-02", "false,false,0,0,0,2;true,false,0,0,0,6")]
		[InlineData("Yaml-01", "false,false,0,0,0,1")]
		[InlineData("Yaml-02", "false,false,0,0,0,2")]
		[InlineData("Yaml-03", "false,false,0,0,0,11")]
		[InlineData("Yaml-04", "false,false,0,0,0,2")]
		[InlineData("Yaml-05", "false,false,0,0,0,6")]
		[InlineData("Yaml-06", "false,false,0,0,0,5")]
		[InlineData("Yaml-07", "false,false,0,0,0,5")]
		[InlineData("Yaml-08", "false,false,0,0,0,3")]
		[InlineData("Yaml-09", "false,false,0,0,0,3")]
		[InlineData("Yaml-10", "false,false,0,0,0,3")]
		[InlineData("Yaml-11", "false,false,0,0,0,3")]
		[InlineData("Yaml-12", "false,false,0,0,0,1;true,false,0,0,0,0;true,true,0,1,0,0;false,false,0,0,0,1")]
		[InlineData("Yaml-14", "false,false,0,0,0,3")]
		[InlineData("Yaml-15", "false,false,0,0,0,25")]
		[InlineData("Yaml-16", "false,false,0,0,0,6")]
		[InlineData("Yaml-17", "false,false,0,0,0,6")]
		[InlineData("Yaml-18", "false,false,0,0,0,6")]
		[InlineData("Yaml-19", "false,false,0,0,0,6")]
		[InlineData("Yaml-50", "true,false,2,1,0,30;true,false,0,0,0,35;true,false,0,0,0,18")]
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
		[InlineData("Documents-02", 1)]
		[InlineData("Documents-03", 7)]
		[InlineData("Documents-04", 9)]
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
		[InlineData("Yaml-15", 1)]
		[InlineData("Yaml-16", 1)]
		[InlineData("Yaml-17", 1)]
		[InlineData("Yaml-18", 1)]
		[InlineData("Yaml-19", 1)]
		[InlineData("Yaml-50", 3)]
		public async Task CreateStream_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);
			var stream = await parser.CreateStream(tokens);

			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
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
		[InlineData("BlockSequence-01", 1, 8)]
		[InlineData("BlockSequence-02", 1, 9)]
		[InlineData("BlockSequence-03", 1, 9)]
		[InlineData("BlockSequence-04", 1, 9)]
		[InlineData("BlockSequence-05", 1, 9)]
		[InlineData("BlockSequence-06", 1, 9)]
		public async Task Parse_BlockSequence_ShouldWorkProperly(string fileName, int expectedNumberOfLevelZeroNodes, int expectedNumberOfDescendants)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();

			var stream = await parser.Parse(value);
			Assert.Equal(expectedNumberOfLevelZeroNodes, stream.Documents[0].Nodes.Count());
			Assert.Equal(expectedNumberOfDescendants, stream.Descendants().Count());
		}

		[Theory]
		[InlineData("Comments-01", "0,0,1")]
		[InlineData("Comments-02", "0,0,14")]
		[InlineData("Comments-03", "0,0,1")]
		[InlineData("Comments-04", "0,0,0;1,0,1")]
		[InlineData("Comments-05", "0,0,0;1,0,0;1,0,1")]
		[InlineData("Comments-06", "0,0,0;1,0,0;1,0,12")]
		[InlineData("Comments-07", "1,0,0;1,0,12")]
		[InlineData("Comments-08", "0,0,1")]
		[InlineData("Comments-09", "0,1,1")]
		[InlineData("Comments-10", "0,0,1;0,0,0;1,0,12")]
		public async Task Parse_Comments_ShouldWorkProperly(string fileName, string expectedResult)
		{
			var value = await GetYaml(fileName);

			var expected = new List<Tuple<int, int, int>>();
			foreach(var pair in expectedResult.Split(';'))
			{
				var numbers = pair.Split(',').Select(item => int.Parse(item.Trim(), null)).ToArray();
				expected.Add(new Tuple<int, int, int>(numbers[0], numbers[1], numbers[2]));
			}

			var parser = await CreateYamlParser();
			var node = await parser.Parse(value);
			Assert.Equal(expected.Count, node.Documents.Count);

			for(var i = 0; i < node.Documents.Count; i++)
			{
				var document = node.Documents.ElementAt(i);
				Assert.Equal(expected[i].Item1, document.LeadingComments.Count);
				Assert.Equal(expected[i].Item1, document.LeadingComments.Count(comment => comment.Value.Trim().StartsWith("Comment ", StringComparison.Ordinal)));
				Assert.Equal(expected[i].Item2, document.TrailingComments.Count);
				Assert.Equal(expected[i].Item2, document.TrailingComments.Count(comment => comment.Value.Trim().StartsWith("Comment ", StringComparison.Ordinal)));
				Assert.Equal(expected[i].Item3, document.Nodes.Count());
			}
		}

		[Theory]
		[InlineData("Documents-Directives-Comments-01", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-02", 1, 2, 0, 1)]
		[InlineData("Documents-Directives-Comments-03", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-04", 1, 2, 0, 1)]
		[InlineData("Documents-Directives-Comments-05", 1, 7, 2, 0)]
		[InlineData("Documents-Directives-Comments-06", 3, 7, 7, 0)]
		public async Task Parse_DocumentsDirectivesComments_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments, int expectedNumberOfDirectives, int expectedNumberOfSurroundingComments, int expectedNumberOfNodes)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
			Assert.Equal(expectedNumberOfDirectives, stream.Documents.Sum(document => document.Directives.Count));
			Assert.Equal(expectedNumberOfSurroundingComments, stream.Documents.Sum(document => document.LeadingComments.Count + document.TrailingComments.Count));
			Assert.Equal(expectedNumberOfNodes, stream.Documents.Sum(document => document.Nodes.Count()));
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
			Assert.Equal(2, secondDocument.Nodes.Count());
			Assert.Empty(secondDocument.LeadingComments);
			Assert.Empty(secondDocument.TrailingComments);

			var firstNode = secondDocument.Nodes.ElementAt(0);
			Assert.Equal(3, firstNode.Children.Count());
			Assert.Single(firstNode.LeadingComments);
			Assert.Equal("Comment 05", firstNode.LeadingComments[0].Value);
			Assert.Empty(firstNode.TrailingComments);

			var secondNode = secondDocument.Nodes.ElementAt(1);
			Assert.Empty(secondNode.Children);
			Assert.Single(secondNode.LeadingComments);
			Assert.Equal("Comment 10", secondNode.LeadingComments[0].Value);
			Assert.Single(secondNode.TrailingComments);
			Assert.Equal("Comment 11", secondNode.TrailingComments[0].Value);
		}

		[Theory]
		[InlineData("Comments-01", 1, 0, 0, 1)]
		[InlineData("Comments-02", 1, 0, 0, 14)]
		[InlineData("Comments-03", 1, 0, 0, 1)]
		[InlineData("Comments-04", 2, 0, 1, 1)]
		[InlineData("Comments-05", 3, 0, 2, 1)]
		[InlineData("Comments-06", 3, 0, 2, 12)]
		[InlineData("Comments-07", 2, 0, 2, 12)]
		[InlineData("Comments-08", 1, 0, 0, 1)]
		[InlineData("Comments-09", 1, 0, 1, 1)]
		[InlineData("Comments-10", 3, 0, 1, 13)]
		[InlineData("Documents-01", 1, 0, 1, 0)]
		[InlineData("Documents-02", 1, 0, 2, 3)]
		[InlineData("Documents-03", 7, 1, 7, 4)]
		[InlineData("Documents-04", 9, 1, 4, 7)]
		[InlineData("Documents-Directives-Comments-01", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-02", 1, 2, 0, 1)]
		[InlineData("Documents-Directives-Comments-03", 1, 2, 0, 0)]
		[InlineData("Documents-Directives-Comments-04", 1, 2, 0, 1)]
		[InlineData("Documents-Directives-Comments-05", 1, 7, 2, 0)]
		[InlineData("Documents-Directives-Comments-06", 3, 7, 7, 0)]
		[InlineData("Empty-01", 0, 0, 0, 0)]
		[InlineData("Empty-02", 0, 0, 0, 0)]
		[InlineData("Nodes-With-Comments-01", 1, 0, 0, 2)]
		[InlineData("Nodes-With-Comments-02", 2, 0, 0, 8)]
		[InlineData("Yaml-01", 1, 0, 0, 1)]
		[InlineData("Yaml-02", 1, 0, 0, 2)]
		[InlineData("Yaml-03", 1, 0, 0, 11)]
		[InlineData("Yaml-04", 1, 0, 0, 2)]
		[InlineData("Yaml-05", 1, 0, 0, 6)]
		[InlineData("Yaml-06", 1, 0, 0, 5)]
		[InlineData("Yaml-07", 1, 0, 0, 5)]
		[InlineData("Yaml-08", 1, 0, 0, 3)]
		[InlineData("Yaml-09", 1, 0, 0, 3)]
		[InlineData("Yaml-10", 1, 0, 0, 3)]
		[InlineData("Yaml-11", 1, 0, 0, 3)]
		[InlineData("Yaml-12", 4, 0, 1, 2)]
		[InlineData("Yaml-14", 1, 0, 0, 3)]
		[InlineData("Yaml-15", 1, 0, 0, 25)]
		[InlineData("Yaml-16", 1, 0, 0, 6)]
		[InlineData("Yaml-17", 1, 0, 0, 6)]
		[InlineData("Yaml-18", 1, 0, 0, 6)]
		[InlineData("Yaml-19", 1, 0, 0, 6)]
		[InlineData("Yaml-50", 3, 2, 1, 83)]
		public async Task Parse_ShouldWorkProperly(string fileName, int expectedNumberOfDocuments, int expectedNumberOfDirectives, int expectedNumberOfSurroundingComments, int expectedNumberOfDescendants)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var stream = await parser.Parse(value);
			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
			Assert.Equal(expectedNumberOfDirectives, stream.Documents.Sum(document => document.Directives.Count));
			Assert.Equal(expectedNumberOfSurroundingComments, stream.Documents.Sum(document => document.LeadingComments.Count + document.TrailingComments.Count));
			Assert.Equal(expectedNumberOfDescendants, stream.Descendants().Count());
		}

		private static async Task ParseNodesWithCommentsShouldWorkProperly(IList<IYamlDocument> documents)
		{
			await Task.CompletedTask;

			var firstDocument = documents[0];
			Assert.Equal(2, firstDocument.Nodes.Count());
			Assert.Empty(firstDocument.LeadingComments);
			Assert.Empty(firstDocument.TrailingComments);

			var firstNode = (IYamlContentNode)firstDocument.Nodes.ElementAt(0);
			Assert.Equal(2, firstNode.LeadingComments.Count);
			Assert.Equal("Comment 01", firstNode.LeadingComments[0].Value);
			Assert.Equal("Comment 02", firstNode.LeadingComments[1].Value);
			Assert.Empty(firstNode.TrailingComments);
			Assert.NotNull(firstNode.Anchor);
			Assert.Equal("b", firstNode.Anchor.Value);
			Assert.Null(firstNode.AnchorAlias);
			Assert.NotNull(firstNode.Comment);
			Assert.True(firstNode.Comment.IsInline);
			Assert.Equal("Comment 03", firstNode.Comment.Value);
			Assert.NotNull(firstNode.Key);
			Assert.Equal(ScalarStyle.Plain, firstNode.Key.Style);
			Assert.True(firstNode.Key.IsKey);
			Assert.Equal("a", firstNode.Key.Value);
			Assert.Null(firstNode.Tag);
			Assert.Null(firstNode.Value);

			var secondNode = (IYamlContentNode)firstDocument.Nodes.ElementAt(1);
			Assert.Empty(secondNode.LeadingComments);
			Assert.Single(secondNode.TrailingComments);
			Assert.Equal("Comment 04", secondNode.TrailingComments[0].Value);
			Assert.Null(secondNode.Anchor);
			Assert.NotNull(secondNode.AnchorAlias);
			Assert.Equal("b", secondNode.AnchorAlias.Value);
			Assert.Null(secondNode.Comment);
			Assert.NotNull(secondNode.Key);
			Assert.Equal(ScalarStyle.Plain, secondNode.Key.Style);
			Assert.True(secondNode.Key.IsKey);
			Assert.Equal("c", secondNode.Key.Value);
			Assert.Null(secondNode.Tag);
			Assert.Null(secondNode.Value);
		}

		[Theory]
		[InlineData("BlockSequence-01", 24, 1, 3)]
		[InlineData("BlockSequence-02", 27, 0, 3)]
		[InlineData("BlockSequence-03", 24, 1, 3)]
		[InlineData("BlockSequence-04", 27, 0, 3)]
		[InlineData("BlockSequence-05", 24, 1, 3)]
		[InlineData("BlockSequence-06", 27, 0, 3)]
		public async Task ParseToTokens_BlockSequence_ShouldWorkProperly(string fileName, int expectedNumberOfTokens, int expectedNumberOfBlockSequenceStart, int expectedNumberOfBlockEntry)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);

			Assert.Equal(expectedNumberOfTokens, tokens.Count);
			Assert.Equal(expectedNumberOfBlockSequenceStart, tokens.Count(token => token is BlockSequenceStart));
			Assert.Equal(expectedNumberOfBlockEntry, tokens.Count(token => token is BlockEntry));
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
		[InlineData("Documents-02", 9)]
		[InlineData("Documents-03", 25)]
		[InlineData("Documents-04", 39)]
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
		[InlineData("Yaml-08", 11)]
		[InlineData("Yaml-09", 12)]
		[InlineData("Yaml-10", 17)]
		[InlineData("Yaml-11", 18)]
		[InlineData("Yaml-12", 18)]
		[InlineData("Yaml-14", 16)]
		[InlineData("Yaml-15", 128)]
		[InlineData("Yaml-16", 28)]
		[InlineData("Yaml-17", 12)]
		[InlineData("Yaml-18", 29)]
		[InlineData("Yaml-19", 15)]
		[InlineData("Yaml-50", 348)]
		public async Task ParseToTokens_ShouldWorkProperly(string fileName, int expectedNumberOfTokens)
		{
			var value = await GetYaml(fileName);

			var parser = await CreateYamlParser();
			var tokens = await parser.ParseToTokens(value);

			Assert.Equal(expectedNumberOfTokens, tokens.Count);
		}

		#endregion
	}
}