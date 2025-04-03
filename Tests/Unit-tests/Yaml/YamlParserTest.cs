using HansKindberg.Text.Formatting.Yaml;
using Shared.Extensions;

namespace UnitTests.Yaml
{
	public class YamlParserTest
	{
		#region Methods

		[Fact]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyYamlNodeCollection()
		{
			var yamlNodes = await new YamlParser().Parse(string.Empty);
			Assert.NotNull(yamlNodes);
			Assert.Empty(yamlNodes);
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
		public async Task Parse_IfTheValueParameterIsOnlyWhiteSpaces_ShouldReturnAnEmptyYamlNodeCollection(string value)
		{
			value = value.ResolveNewLine();

			var yamlNodes = await new YamlParser().Parse(value);
			Assert.NotNull(yamlNodes);
			Assert.Empty(yamlNodes);
		}

		/// <summary>
		/// https://yaml.org/spec/1.2.2/#22-structures
		/// </summary>
		[Theory]
		[InlineData("firstProperty: \"First value\"", 1)]
		[InlineData("firstProperty: \"First value\"\nsecondProperty: \"Second value\"\nthirdProperty: \"Third value\"", 1)]
		[InlineData("---\nfirstRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nsecondRootProperty:\n  firstSubProperty: \"First sub value\"\n---\nthirdRootProperty:\n  firstSubProperty: \"First sub value\"", 3)]
		public async Task Parse_ShouldWorkProperly(string value, int expectedNumberOfNodes)
		{
			value = value.ResolveNewLine();

			var yamlNodes = await new YamlParser().Parse(value);
			Assert.NotNull(yamlNodes);
			Assert.Equal(expectedNumberOfNodes, yamlNodes.Count);
		}

		#endregion
	}
}