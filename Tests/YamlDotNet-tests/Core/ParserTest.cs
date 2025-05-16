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
		[InlineData("Comments-01")]
		public async Task Current_Iteration_Comments_Test(string fileName)
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

			Assert.Equal(2, parsingEvents.Count);
			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is Comment);
			Assert.Equal("First comment", ((Comment)parsingEvents[1]).Value);
		}

		[Theory]
		[InlineData("Documents-01", 2, 0, 0, 0, 0)]
		[InlineData("Documents-02", 5, 1, 1, 0, 1)]
		[InlineData("Documents-03", 2, 0, 0, 0, 0)]
		[InlineData("Documents-04", 11, 3, 3, 0, 3)]
		[InlineData("Documents-05", 5, 1, 1, 0, 0)]
		[InlineData("Documents-06", 11, 3, 3, 0, 0)]
		public async Task Current_Iteration_Documents_Test(string fileName, int expectedNumberOfParsingEvents, int expectedNumberOfDocumentStarts, int expectedNumberOfDocumentEnds, int expectedNumberOfImplicitDocumentStarts, int expectedNumberOfImplicitDocumentEnds)
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

			Assert.Equal(expectedNumberOfParsingEvents, parsingEvents.Count);
			Assert.Equal(expectedNumberOfDocumentStarts, parsingEvents.OfType<DocumentStart>().Count());
			Assert.Equal(expectedNumberOfDocumentEnds, parsingEvents.OfType<DocumentEnd>().Count());
			Assert.Equal(expectedNumberOfImplicitDocumentStarts, parsingEvents.OfType<DocumentStart>().Count(documentStart => documentStart.IsImplicit));
			Assert.Equal(expectedNumberOfImplicitDocumentEnds, parsingEvents.OfType<DocumentEnd>().Count(documentEnd => documentEnd.IsImplicit));
		}

		[Theory]
		[InlineData("Yaml-01")]
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

		private static async Task<string> GetFileText(string path)
		{
			await Task.CompletedTask;

			// ReSharper disable MethodHasAsyncOverload
			return File.ReadAllText(path).ResolveNewLine();
			// ReSharper restore MethodHasAsyncOverload
		}

		private static async Task<string> GetYaml(string fileName)
		{
			return await GetFileText(Path.Combine(_resourcesDirectory.FullName, $"{fileName}.yml"));
		}

		#endregion
	}
}