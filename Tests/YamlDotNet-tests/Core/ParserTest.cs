using Shared.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNetTests.Core
{
	public class ParserTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Core", "Resources", "ParserTest"));

		#endregion

		#region Methods

		[Theory]
		[InlineData("Yaml-01.yml")]
		public async Task Current_Iteration_Test(string fileName)
		{
			await Task.CompletedTask;

			var text = await GetYaml(fileName);

			var parsingEvents = new List<ParsingEvent>();

			using(var stringReader = new StringReader(text))
			{
				var scanner = new Scanner(stringReader, false);
				var parser = new Parser(scanner);

				while(parser.MoveNext())
				{
					parsingEvents.Add(parser.Current!);
				}
			}

			Assert.Equal(83, parsingEvents.Count);
			Assert.Equal(15, parsingEvents.OfType<Comment>().Count());
			Assert.Equal(3, parsingEvents.OfType<DocumentEnd>().Count());
			Assert.Equal(3, parsingEvents.OfType<DocumentStart>().Count());
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