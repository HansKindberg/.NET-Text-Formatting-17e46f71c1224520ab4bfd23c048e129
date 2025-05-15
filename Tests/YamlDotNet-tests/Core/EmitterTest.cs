using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNetTests.Core
{
	public class EmitterTest
	{
		#region Methods

		[Theory]
		[InlineData("key: &anchor !tag value", "key: &anchor !tag value")]
		[InlineData("key: !tag &anchor value", "key: &anchor !tag value")]
		[InlineData("key: value &anchor !tag", "key: value &anchor !tag")]
		[InlineData("    key    :     &anchor     !tag     value    ", "key: &anchor !tag value")]
		[InlineData("    key    :     !tag     &anchor     value    ", "key: &anchor !tag value")]
		[InlineData("    key    :     value     &anchor     !tag    ", "key: value     &anchor     !tag")]
		public async Task Emit_Test(string yaml, string expectedYaml)
		{
			await Task.CompletedTask;

			var parsingEvents = new List<ParsingEvent>();

			using(var stringReader = new StringReader(yaml))
			{
				var scanner = new Scanner(stringReader, false);
				var parser = new Parser(scanner);

				while(parser.MoveNext())
				{
					parsingEvents.Add(parser.Current!);
				}
			}

			var stringBuilder = new StringBuilder();
			using(var stringWriter = new StringWriter(stringBuilder))
			{
				var emitter = new Emitter(stringWriter);

				foreach(var parsingEvent in parsingEvents)
				{
					emitter.Emit(parsingEvent);
				}
			}

			Assert.Equal(expectedYaml + Environment.NewLine, stringBuilder.ToString());
		}

		#endregion
	}
}