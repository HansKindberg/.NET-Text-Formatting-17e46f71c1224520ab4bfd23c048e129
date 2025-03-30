using System.Text.Json;
using Shared.Extensions;

namespace JsonTests
{
	public class JsonSerializerTest
	{
		#region Methods

		[Theory]
		[InlineData("{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}", "{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}")]
		[InlineData("{ \"a\": \"b\", \"c\": [ { \"d\": \"e\" }, { \"f\": \"g\" } ] }", "{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}")]
		public async Task Serialize_JsonElement_ShouldWorkProperly(string json, string expected)
		{
			await Task.CompletedTask;

			json = json.ResolveNewLine();
			expected = expected.ResolveNewLine();

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

			var serializedJson = JsonSerializer.Serialize(jsonElement);

			Assert.Equal(expected, serializedJson);
		}

		[Theory]
		[InlineData("{\"a\":\"b\",\"c\":{\"d\":\"e\",\"f\":\"g\"},\"h\":[{\"i\":\"j\"},{\"k\":\"l\"}]}", "{\n\t\t\t\t\"a\": \"b\",\n\t\t\t\t\"c\": {\n\t\t\t\t\t\t\t\t\"d\": \"e\",\n\t\t\t\t\t\t\t\t\"f\": \"g\"\n\t\t\t\t},\n\t\t\t\t\"h\": [\n\t\t\t\t\t\t\t\t{\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\n\t\t\t\t\t\t\t\t},\n\t\t\t\t\t\t\t\t{\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\n\t\t\t\t\t\t\t\t}\n\t\t\t\t]\n}")]
		[InlineData("{ \"a\": \"b\", \"c\": { \"d\": \"e\", \"f\": \"g\" }, \"h\": [ { \"i\": \"j\" }, { \"k\": \"l\" } ] }", "{\n\t\t\t\t\"a\": \"b\",\n\t\t\t\t\"c\": {\n\t\t\t\t\t\t\t\t\"d\": \"e\",\n\t\t\t\t\t\t\t\t\"f\": \"g\"\n\t\t\t\t},\n\t\t\t\t\"h\": [\n\t\t\t\t\t\t\t\t{\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\n\t\t\t\t\t\t\t\t},\n\t\t\t\t\t\t\t\t{\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\n\t\t\t\t\t\t\t\t}\n\t\t\t\t]\n}")]
		public async Task Serialize_JsonElement_WithIndentation_ShouldWorkProperly(string json, string expected)
		{
			await Task.CompletedTask;

			json = json.ResolveNewLine();
			expected = expected.ResolveNewLine();

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

#pragma warning disable CA1869
			var serializedJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { IndentCharacter = '\t', IndentSize = 4, WriteIndented = true });
#pragma warning restore CA1869

			Assert.Equal(expected, serializedJson);
		}

		#endregion
	}
}