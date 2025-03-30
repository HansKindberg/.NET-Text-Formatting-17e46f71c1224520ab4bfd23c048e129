using HansKindberg.Text.Formatting.Yaml;
using Shared.Extensions;

namespace UnitTests.Yaml
{
	// TODO: Look over this test-class.
	public class YamlParserTest
	{
		#region Methods

		[Fact]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyYamlDocument()
		{
			var yamlDocument = await new YamlParser().Parse(string.Empty);
			Assert.NotNull(yamlDocument);
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
		public async Task Parse_IfTheValueParameterIsOnlyWhiteSpaces_ShouldReturnAnEmptyYamlDocument(string value)
		{
			value = value.ResolveNewLine();

			var yamlDocument = await new YamlParser().Parse(value);
			Assert.NotNull(yamlDocument);
		}

		[Theory]
		[InlineData("firstProperty: \"First value\"")]
		public async Task Parse_ShouldWorkProperly(string value)
		{
			value = value.ResolveNewLine();

			var yamlDocument = await new YamlParser().Parse(value);
			Assert.NotNull(yamlDocument);
		}

		#endregion
	}
}