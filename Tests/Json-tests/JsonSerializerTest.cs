using System.Text.Json;

namespace JsonTests
{
	public class JsonSerializerTest
	{
		#region Methods

		[Theory]
		[InlineData("{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}", "{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}")]
		[InlineData("{ \"a\": \"b\", \"c\": [ { \"d\": \"e\" }, { \"f\": \"g\" } ] }", "{\"a\":\"b\",\"c\":[{\"d\":\"e\"},{\"f\":\"g\"}]}")]
		public async Task Serialize_JsonElement_ShouldWorkProperly(string json, string expectedResult)
		{
			await Task.CompletedTask;

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

			var serializedJson = JsonSerializer.Serialize(jsonElement);

			Assert.Equal(expectedResult, serializedJson);
		}

		[Theory]
		[InlineData("{\"a\":\"b\",\"c\":{\"d\":\"e\",\"f\":\"g\"},\"h\":[{\"i\":\"j\"},{\"k\":\"l\"}]}", "{\r\n\t\t\t\t\"a\": \"b\",\r\n\t\t\t\t\"c\": {\r\n\t\t\t\t\t\t\t\t\"d\": \"e\",\r\n\t\t\t\t\t\t\t\t\"f\": \"g\"\r\n\t\t\t\t},\r\n\t\t\t\t\"h\": [\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\r\n\t\t\t\t\t\t\t\t},\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\r\n\t\t\t\t\t\t\t\t}\r\n\t\t\t\t]\r\n}")]
		[InlineData("{ \"a\": \"b\", \"c\": { \"d\": \"e\", \"f\": \"g\" }, \"h\": [ { \"i\": \"j\" }, { \"k\": \"l\" } ] }", "{\r\n\t\t\t\t\"a\": \"b\",\r\n\t\t\t\t\"c\": {\r\n\t\t\t\t\t\t\t\t\"d\": \"e\",\r\n\t\t\t\t\t\t\t\t\"f\": \"g\"\r\n\t\t\t\t},\r\n\t\t\t\t\"h\": [\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\r\n\t\t\t\t\t\t\t\t},\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\r\n\t\t\t\t\t\t\t\t}\r\n\t\t\t\t]\r\n}")]
		public async Task Serialize_JsonElement_WithIndentation_ShouldWorkProperly(string json, string expectedResult)
		{
			await Task.CompletedTask;

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

#pragma warning disable CA1869
			var serializedJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { IndentCharacter = '\t', IndentSize = 4, WriteIndented = true });
#pragma warning restore CA1869

			Assert.Equal(expectedResult, serializedJson);
		}

		#endregion
	}
}