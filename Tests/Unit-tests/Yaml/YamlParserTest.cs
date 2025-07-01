using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models.Extensions;
using Shared.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace UnitTests.Yaml
{
	public class YamlParserTest
	{
		#region Methods

		[Theory]
		[InlineData("[1]", 1)]
		[InlineData("[1,2]", 2)]
		[InlineData("[a,b,c,d]", 4)]
		[InlineData("[a, b, c, d]", 4)]
		[InlineData("[{a: b},{c: d},{e: f}]", 3)]
		public async Task AddMissingFlowEntries_ShouldWorkProperly(string value, int expectedNumberOfFlowEntries)
		{
			var tokens = ParseToTokens(value);
			Assert.Equal(expectedNumberOfFlowEntries - 1, tokens.Count(token => token is FlowEntry));

			await new YamlParser().AddMissingFlowEntries(tokens);
			Assert.Equal(expectedNumberOfFlowEntries, tokens.Count(token => token is FlowEntry));
		}

		[Fact]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyYamlStream()
		{
			var stream = await new YamlParser().Parse(string.Empty);
			Assert.NotNull(stream);
			Assert.Empty(stream.Documents);
		}

		[Fact]
		public async Task Parse_IfTheValueParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await new YamlParser().Parse(null!));
			Assert.Equal("value", argumentNullException.ParamName);
		}

		[Theory]
		[InlineData(" ")]
		[InlineData("     ")]
		[InlineData("         \n         \n          \t    ")]
		public async Task Parse_IfTheValueParameterIsOnlyWhiteSpaces_ShouldReturnAnEmptyYamlStream(string value)
		{
			value = value.ResolveNewLine();

			var stream = await new YamlParser().Parse(value);
			Assert.NotNull(stream);
			Assert.Empty(stream.Documents);
		}

		/// <summary>
		/// https://yaml.org/spec/1.2.2/#22-structures
		/// </summary>
		[Theory]
		[InlineData("firstProperty: \"First value\"", 2, 1)]
		[InlineData("firstProperty: \"First value\"\nsecondProperty: \"Second value\"\nthirdProperty: \"Third value\"", 4, 1)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"", 12, 3)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...", 12, 3)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...\n...", 12, 4)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...\n...\n...\n...\n...\n... # Comment 1\n... # Comment 2\n---\n--- # Comment 3\n...", 12, 11)]
		public async Task Parse_ShouldWorkProperly(string value, int expectedNumberOfDescendants, int expectedNumberOfDocuments)
		{
			value = value.ResolveNewLine();

			var stream = await new YamlParser().Parse(value);
			Assert.NotNull(stream);
			Assert.Equal(expectedNumberOfDescendants, stream.Descendants().Count());
			Assert.Equal(expectedNumberOfDocuments, stream.Documents.Count);
		}

		private static List<Token> ParseToTokens(string value)
		{
			var tokens = new List<Token>();

			using(var stringReader = new StringReader(value))
			{
				var scanner = new Scanner(stringReader, false);

				while(scanner.MoveNext())
				{
					tokens.Add(scanner.Current!);
				}
			}

			return tokens;
		}

		#endregion
	}
}