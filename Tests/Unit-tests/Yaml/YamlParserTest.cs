using HansKindberg.Text.Formatting.Yaml;
using Shared.Extensions;

namespace UnitTests.Yaml
{
	public class YamlParserTest
	{
		#region Methods

		[Fact]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyYamlNode()
		{
			var yamlNode = await new YamlParser().Parse(string.Empty);
			Assert.NotNull(yamlNode);
			Assert.Empty(yamlNode.Children);
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
		public async Task Parse_IfTheValueParameterIsOnlyWhiteSpaces_ShouldReturnAnEmptyYamlNode(string value)
		{
			value = value.ResolveNewLine();

			var yamlNode = await new YamlParser().Parse(value);
			Assert.NotNull(yamlNode);
			Assert.Empty(yamlNode.Children);
		}

		/// <summary>
		/// https://yaml.org/spec/1.2.2/#22-structures
		/// </summary>
		[Theory]
		[InlineData("firstProperty: \"First value\"", 1)]
		[InlineData("firstProperty: \"First value\"\nsecondProperty: \"Second value\"\nthirdProperty: \"Third value\"", 1)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"", 3)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...", 3)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...\n...", 3)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"\n...\n...\n...\n...\n...\n... # Comment 1\n... # Comment 2\n---\n--- # Comment 3\n...", 5)]
		public async Task Parse_ShouldWorkProperly(string value, int expectedNumberOfNodes)
		{
			value = value.ResolveNewLine();

			var yamlNode = await new YamlParser().Parse(value);
			Assert.NotNull(yamlNode);
			Assert.Equal(expectedNumberOfNodes, yamlNode.Children.Count());
		}

		#endregion
	}
}