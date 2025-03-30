using System.Text.Json;
using HansKindberg.Text.Formatting.Extensions;

namespace HansKindberg.Text.Formatting.Json
{
	public class JsonParser : IParser<JsonElement>
	{
		#region Fields

		private const int _informationalJsonMaximumLength = 100;

		#endregion

		#region Methods

		protected internal virtual string GetStringRepresentation(string? value)
		{
			if(value is { Length: > _informationalJsonMaximumLength })
				value = $"{value.Substring(0, _informationalJsonMaximumLength)}...";

			return value.ToStringRepresentation();
		}

		public virtual async Task<JsonElement> Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				await Task.CompletedTask;

				var jsonElement = JsonSerializer.Deserialize<JsonElement>(value);

				return jsonElement;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not parse the value {this.GetStringRepresentation(value)} to json-element.", exception);
			}
		}

		#endregion
	}
}