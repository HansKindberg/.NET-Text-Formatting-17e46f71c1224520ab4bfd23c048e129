using System.ComponentModel;
using HansKindberg.Text.Formatting.Configuration;
using HansKindberg.Text.Formatting.Json;
using HansKindberg.Text.Formatting.Json.Configuration;
using Shared.Extensions;

namespace UnitTests.Json
{
	// TODO: Look over this test-class.
	public class JsonFormatterTest
	{
		#region Methods

		private static async Task<JsonFormatOptions> CreateJsonFormatOptions(char indentationCharacter, bool indentationEnabled, byte indentationSize, bool sortingEnabled, ListSortDirection sortingDirection)
		{
			await Task.CompletedTask;

			return new JsonFormatOptions
			{
				Indentation = new IndentationOptions
				{
					Character = indentationCharacter,
					Enabled = indentationEnabled,
					Size = indentationSize
				},
				Sorting = new SortingOptions
				{
					Direction = sortingDirection,
					Enabled = sortingEnabled
				}
			};
		}

		[Theory]
		[InlineData("{\"a\":\"b\",\"c\":{\"d\":\"e\",\"f\":\"g\"},\"h\":[{\"i\":\"j\"},{\"k\":\"l\"}]}", "{\r\n\t\"a\": \"b\",\r\n\t\"c\": {\r\n\t\t\"d\": \"e\",\r\n\t\t\"f\": \"g\"\r\n\t},\r\n\t\"h\": [\r\n\t\t{\r\n\t\t\t\"i\": \"j\"\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"k\": \"l\"\r\n\t\t}\r\n\t]\r\n}", '\t', true, 1, true, ListSortDirection.Ascending)]
		[InlineData("{\"a\":\"b\",\"c\":{\"d\":\"e\",\"f\":\"g\"},\"h\":[{\"i\":\"j\"},{\"k\":\"l\"}]}", "{\r\n\t\t\t\t\"a\": \"b\",\r\n\t\t\t\t\"c\": {\r\n\t\t\t\t\t\t\t\t\"d\": \"e\",\r\n\t\t\t\t\t\t\t\t\"f\": \"g\"\r\n\t\t\t\t},\r\n\t\t\t\t\"h\": [\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\r\n\t\t\t\t\t\t\t\t},\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\r\n\t\t\t\t\t\t\t\t}\r\n\t\t\t\t]\r\n}", '\t', true, 4, true, ListSortDirection.Ascending)]

		//[InlineData("{ \"a\": \"b\", \"c\": { \"d\": \"e\", \"f\": \"g\" }, \"h\": [ { \"i\": \"j\" }, { \"k\": \"l\" } ] }", "{\r\n\t\t\t\t\"a\": \"b\",\r\n\t\t\t\t\"c\": {\r\n\t\t\t\t\t\t\t\t\"d\": \"e\",\r\n\t\t\t\t\t\t\t\t\"f\": \"g\"\r\n\t\t\t\t},\r\n\t\t\t\t\"h\": [\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"i\": \"j\"\r\n\t\t\t\t\t\t\t\t},\r\n\t\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"k\": \"l\"\r\n\t\t\t\t\t\t\t\t}\r\n\t\t\t\t]\r\n}")]
		public async Task Format_ShouldWorkProperly(string text, string expected, char indentationCharacter, bool indentationEnabled, byte indentationSize, bool sortingEnabled, ListSortDirection sortingDirection)
		{
			text = text.ResolveNewLine();
			expected = expected.ResolveNewLine();

			var options = await CreateJsonFormatOptions(indentationCharacter, indentationEnabled, indentationSize, sortingEnabled, sortingDirection);

			var jsonFormatter = new JsonFormatter(new JsonParser());
			var formatted = await jsonFormatter.Format(options, text);
			Assert.Equal(expected, formatted);
		}

		#endregion
	}
}