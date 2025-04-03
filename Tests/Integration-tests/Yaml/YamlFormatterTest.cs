using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using Shared.Extensions;

namespace IntegrationTests.Yaml
{
	public class YamlFormatterTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlFormatterTest"));

		#endregion

		#region Methods

		private static async Task<YamlFormatter> CreateYamlFormatter()
		{
			await Task.CompletedTask;

			return new YamlFormatter(new YamlParser());
		}

		[Fact]
		public async Task Format_IfTheTextParameterIsAnEmptyString_ShouldReturnAnEmptyString()
		{
			var yamlFormatter = await CreateYamlFormatter();
			var formattedText = await yamlFormatter.Format(new YamlFormatOptions(), string.Empty);
			Assert.NotNull(formattedText);
			Assert.Equal(string.Empty, formattedText);
		}

		[Fact]
		public async Task Format_IfTheTextParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var yamlFormatter = await CreateYamlFormatter();
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await yamlFormatter.Format(new YamlFormatOptions(), null!));
			Assert.NotNull(argumentNullException);
			Assert.Equal("text", argumentNullException.ParamName);
		}

		[Theory]
		[InlineData("Yaml-01.yml")]
		//[InlineData("Yaml-02.yml")]
		//[InlineData("Yaml-03.yml")]
		//[InlineData("Yaml-04.yml")]
		//[InlineData("Yaml-05.yml")]
		//[InlineData("Yaml-06.yml")]
		//[InlineData("Yaml-07.yml")]
		//[InlineData("Yaml-08.yml")]
		//[InlineData("Yaml-09.yml")]
		[InlineData("Yaml-10.yml")]
		//[InlineData("Yaml-11.yml")]
		//[InlineData("Yaml-12.yml")]
		//[InlineData("Yaml-13.yml")]
		//[InlineData("Yaml-14.yml")]
		//[InlineData("Yaml-15.yml")]
		//[InlineData("Yaml-16.yml")]
		//[InlineData("Yaml-17.yml")]
		//[InlineData("Yaml-18.yml")]
		//[InlineData("Yaml-19.yml")]
		//[InlineData("Yaml-20.yml")]
		public async Task Format_ShouldWorkProperly(string fileName)
		{
			var text = await GetYaml(fileName);
			var expectedText = await GetExpectedYaml(fileName);

			var yamlFormatter = await CreateYamlFormatter();
			var formattedText = await yamlFormatter.Format(new YamlFormatOptions(), text);
			Assert.Equal(expectedText, formattedText);
		}

		private static async Task<string> GetExpectedYaml(string fileName)
		{
			return await GetYamlByPath(Path.Combine(_resourcesDirectory.FullName, "Expected", fileName));
		}

		private static async Task<string> GetYaml(string fileName)
		{
			return await GetYamlByPath(Path.Combine(_resourcesDirectory.FullName, fileName));
		}

		private static async Task<string> GetYamlByPath(string path)
		{
			await Task.CompletedTask;

			// ReSharper disable MethodHasAsyncOverload
			return File.ReadAllText(path).ResolveNewLine();
			// ReSharper restore MethodHasAsyncOverload
		}

		#endregion
	}
}