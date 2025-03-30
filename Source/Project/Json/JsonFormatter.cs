using System.ComponentModel;
using System.Text.Json;
using HansKindberg.Text.Formatting.Configuration;
using HansKindberg.Text.Formatting.Json.Configuration;
using static System.Text.Json.JsonElement;

namespace HansKindberg.Text.Formatting.Json
{
	// TODO: Look over this class.
	///// <inheritdoc />
	public class JsonFormatter(IParser<JsonElement> parser)
	{
		#region Properties

		protected internal virtual IParser<JsonElement> Parser { get; } = parser ?? throw new ArgumentNullException(nameof(parser));

		#endregion

		#region Methods

		protected internal virtual async Task<JsonElement> ConvertToJsonElement<T>(T instance)
		{
			var json = JsonSerializer.Serialize(instance);

			return await this.Parser.Parse(json);
		}

		protected internal virtual async Task<JsonSerializerOptions> CreateJsonSerializerOptions(JsonFormatOptions options)
		{
			await Task.CompletedTask;

			return new JsonSerializerOptions
			{
				IndentCharacter = options.Indentation.Character,
				IndentSize = options.Indentation.Size,
				WriteIndented = options.Indentation.Enabled
			};
		}

		public virtual async Task<string> Format(JsonFormatOptions options, string text)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(text == null)
				throw new ArgumentNullException(nameof(text));

			var jsonElement = await this.Parser.Parse(text);

			if(options.Sorting.Enabled)
				jsonElement = await this.Sort(jsonElement, options.Sorting);

			return JsonSerializer.Serialize(jsonElement, await this.CreateJsonSerializerOptions(options));
		}

		protected internal virtual async Task<IEnumerable<JsonProperty>> Sort(ObjectEnumerator jsonProperties, SortingOptions options)
		{
			await Task.CompletedTask;

			if(!options.Enabled)
				return jsonProperties;

			if(options.Direction == ListSortDirection.Ascending)
				return jsonProperties.OrderBy(property => property.Name);

			return jsonProperties.OrderByDescending(property => property.Name);
		}

		protected internal virtual async Task<JsonElement> Sort(JsonElement jsonElement, SortingOptions options)
		{
			if(jsonElement.ValueKind == JsonValueKind.Object)
			{
				var sorted = new Dictionary<string, JsonElement>();

				foreach(var property in await this.Sort(jsonElement.EnumerateObject(), options))
				{
					sorted[property.Name] = await this.Sort(property.Value, options);
				}

				return await this.ConvertToJsonElement(sorted);
			}

			if(jsonElement.ValueKind == JsonValueKind.Array)
			{
				var sorted = new List<JsonElement>();

				foreach(var item in jsonElement.EnumerateArray())
				{
					sorted.Add(await this.Sort(item, options));
				}

				return await this.ConvertToJsonElement(sorted);
			}

			return jsonElement;
		}

		#endregion
	}
}