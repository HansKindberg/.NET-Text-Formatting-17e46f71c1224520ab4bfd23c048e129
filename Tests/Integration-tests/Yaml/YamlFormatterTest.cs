using System.Text.Json;
using System.Text.Json.Serialization;
using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using Shared.Extensions;

namespace IntegrationTests.Yaml
{
	public class YamlFormatterTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Yaml", "Resources", "YamlFormatterTest"));
		private static readonly JsonSerializerOptions _jsonSerializerOptions = CreateJsonSerializerOptions();

		#endregion

		#region Methods

		private static JsonSerializerOptions CreateJsonSerializerOptions()
		{
			var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);

			jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

			return jsonSerializerOptions;
		}

		private static async Task<YamlFormatter> CreateYamlFormatter()
		{
			await Task.CompletedTask;

			return new YamlFormatter(new YamlParser());
		}

		[Theory]
		//[InlineData("Comments-01", DocumentNotation.None)]
		//[InlineData("Comments-01", DocumentNotation.Minimal)]
		//[InlineData("Comments-01", DocumentNotation.ForceDocumentEnd)]
		//[InlineData("Comments-01", DocumentNotation.ForceDocumentStart)]
		//[InlineData("Comments-01", DocumentNotation.ForceDocumentEndAndDocumentStart)]
		//[InlineData("Comments-02", DocumentNotation.None)]
		//[InlineData("Comments-02", DocumentNotation.Minimal)]
		//[InlineData("Comments-02", DocumentNotation.ForceDocumentEnd)]
		//[InlineData("Comments-02", DocumentNotation.ForceDocumentStart)]
		//[InlineData("Comments-02", DocumentNotation.ForceDocumentEndAndDocumentStart)]
		//[InlineData("Comments-03", DocumentNotation.None)]
		//[InlineData("Comments-03", DocumentNotation.Minimal)]
		//[InlineData("Comments-03", DocumentNotation.ForceDocumentEnd)]
		//[InlineData("Comments-03", DocumentNotation.ForceDocumentStart)]
		//[InlineData("Comments-03", DocumentNotation.ForceDocumentEndAndDocumentStart)]
		//[InlineData("Comments-04", DocumentNotation.None)]
		//[InlineData("Comments-04", DocumentNotation.Minimal)]
		//[InlineData("Comments-04", DocumentNotation.ForceDocumentEnd)]
		//[InlineData("Comments-04", DocumentNotation.ForceDocumentStart)]
		//[InlineData("Comments-04", DocumentNotation.ForceDocumentEndAndDocumentStart)]

		[InlineData("Comments-05", DocumentNotation.None)]
		//////////////////[InlineData("Comments-05", DocumentNotation.Minimal)]
		//////////////////[InlineData("Comments-05", DocumentNotation.ForceDocumentEnd)]
		//////////////////[InlineData("Comments-05", DocumentNotation.ForceDocumentStart)]
		//////////////////[InlineData("Comments-05", DocumentNotation.ForceDocumentEndAndDocumentStart)]

		public async Task Format_Comments_ShouldWorkProperly(string fileName, DocumentNotation documentNotation)
		{
			var text = await GetYaml(fileName);
			var expectedText = await GetExpectedYaml($"{fileName}-DocumentNotation-{documentNotation}");

			var yamlFormatter = await CreateYamlFormatter();
			var formattedText = await yamlFormatter.Format(new YamlFormatOptions{DocumentNotation = documentNotation}, text);
			Assert.Equal(expectedText, formattedText);
		}

		[Theory]
		[InlineData("Empty-01")]
		[InlineData("Empty-02")]
		public async Task Format_Empty_ShouldWorkProperly(string fileName)
		{
			var text = await GetYaml(fileName);
			var expectedText = await GetExpectedYaml($"{fileName}");

			var yamlFormatter = await CreateYamlFormatter();
			var formattedText = await yamlFormatter.Format(new YamlFormatOptions(), text);
			Assert.Equal(expectedText, formattedText);
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
		//[InlineData("Yaml-01", NamingConvention.CamelCase, true, ListSortDirection.Ascending)]
		//[InlineData("Yaml-01", "Options-01")]
		//[InlineData("Yaml-02")]
		//[InlineData("Yaml-03")]
		//[InlineData("Yaml-04")]
		//[InlineData("Yaml-05")]
		//[InlineData("Yaml-06")]
		//[InlineData("Yaml-07")]
		//[InlineData("Yaml-08")]
		//[InlineData("Yaml-09")]
		//[InlineData("Yaml-10", NamingConvention.CamelCase, true, ListSortDirection.Ascending)]
		//[InlineData("Yaml-11", NamingConvention.Underscored, true, ListSortDirection.Ascending)]
		[InlineData("Yaml-11", "Options-03")]
		//[InlineData("Yaml-11")]
		//[InlineData("Yaml-12")]
		//[InlineData("Yaml-13")]
		//[InlineData("Yaml-14")]
		//[InlineData("Yaml-15")]
		//[InlineData("Yaml-16")]
		//[InlineData("Yaml-17")]
		//[InlineData("Yaml-18")]
		//[InlineData("Yaml-19")]
		//[InlineData("Yaml-20")]
		public async Task Format_ShouldWorkProperly(string yamlFileName, string optionsFileName)
		{
			var expectedText = await GetExpectedYaml($"{yamlFileName}-{optionsFileName}");
			var options = await GetOptions(optionsFileName);
			var text = await GetYaml(yamlFileName);

			var yamlFormatter = await CreateYamlFormatter();
			var formattedText = await yamlFormatter.Format(options, text);
			Assert.NotNull(formattedText);
			Assert.Equal(expectedText, formattedText);
		}

		private static async Task<string> GetExpectedYaml(string fileName)
		{
			return await GetFileText(Path.Combine(_resourcesDirectory.FullName, "Expected", $"{fileName}.yml"));
		}

		private static async Task<string> GetFileText(string path)
		{
			await Task.CompletedTask;

			// ReSharper disable MethodHasAsyncOverload
			return File.ReadAllText(path).ResolveNewLine();
			// ReSharper restore MethodHasAsyncOverload
		}

		private static async Task<YamlFormatOptions> GetOptions(string fileName)
		{
			var text = await GetFileText(Path.Combine(_resourcesDirectory.FullName, "Options", $"{fileName}.json"));

			return JsonSerializer.Deserialize<YamlFormatOptions>(text, _jsonSerializerOptions)!;
		}

		private static async Task<string> GetYaml(string fileName)
		{
			return await GetFileText(Path.Combine(_resourcesDirectory.FullName, $"{fileName}.yml"));
		}

		#endregion
	}
}