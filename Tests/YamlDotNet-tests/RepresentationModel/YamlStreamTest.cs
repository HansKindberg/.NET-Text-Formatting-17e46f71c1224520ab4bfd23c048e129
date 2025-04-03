using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace YamlDotNetTests.RepresentationModel
{
	public class YamlStreamTest
	{
		#region Methods

		private static async Task<IParser> CreateParser(TextReader textReader)
		{
			return new Parser(await CreateScanner(textReader));
		}

		private static async Task<IScanner> CreateScanner(TextReader textReader)
		{
			await Task.CompletedTask;

			return new Scanner(textReader, false);
		}

		[Fact]
		public async Task Load_WithComments_IsNotSupportedYet()
		{
			await Task.CompletedTask;

			const string yaml = "a: b # Comment";

			var yamlStream = new YamlStream();

			using(var stringReader = new StringReader(yaml))
			{
				var argumentException = await Assert.ThrowsAsync<ArgumentException>(async () => yamlStream.Load(await CreateParser(stringReader)));

				Assert.NotNull(argumentException);
				Assert.Equal("parser", argumentException.ParamName);
				Assert.StartsWith("The current event is of an unsupported type.", argumentException.Message);
			}
		}

		#endregion
	}
}