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

		[Fact]
		public async Task Current_Iteration_DocumentEndOnly_ShouldResultInAStreamStartAndAStreamEnd()
		{
			await Task.CompletedTask;

			const string text = "...";

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
			Assert.True(parsingEvents[1] is StreamEnd);
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

		[Fact]
		public async Task Current_Iteration_DocumentsDirectiveComments_Test1()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Documents-Directives-Comments-01");

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

			Assert.Equal(14, parsingEvents.Count);
			Assert.Equal(typeof(StreamStart), parsingEvents[0].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[1].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[2].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[3].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[4].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[5].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[6].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[7].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[8].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[9].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[10].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[11].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[12].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[13].GetType());
		}

		[Fact]
		public async Task Current_Iteration_DocumentsDirectiveComments_Test2()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Documents-Directives-Comments-02");

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

			Assert.Equal(22, parsingEvents.Count);
			Assert.Equal(typeof(StreamStart), parsingEvents[0].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[1].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[2].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[3].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[4].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[5].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[6].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[7].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[8].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[9].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[10].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[11].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[12].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[13].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[14].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[15].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[16].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[17].GetType());
			Assert.Equal(typeof(Comment), parsingEvents[18].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[19].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[20].GetType());
			Assert.Equal(typeof(StreamEnd), parsingEvents[21].GetType());
		}

		[Fact]
		public async Task Current_Iteration_DocumentsSpecial_Test1()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Documents-Special-01");

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

			Assert.Equal(14, parsingEvents.Count);
			Assert.Equal(typeof(StreamStart), parsingEvents[0].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[1].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[2].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[3].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[4].GetType());
			Assert.Equal(typeof(MappingStart), parsingEvents[5].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[6].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[7].GetType());
			Assert.Equal(typeof(MappingEnd), parsingEvents[8].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[9].GetType());
			Assert.Equal(typeof(DocumentStart), parsingEvents[10].GetType());
			Assert.Equal(typeof(Scalar), parsingEvents[11].GetType());
			Assert.Equal(typeof(DocumentEnd), parsingEvents[12].GetType());
			Assert.Equal(typeof(StreamEnd), parsingEvents[13].GetType());
		}

		[Fact]
		public async Task Current_Iteration_DocumentStartAndDocumentEndOnSameLine_ShouldResultInAStreamStartAndADocumentStartAndAScalarAndADocumentEndAndAStreamEnd()
		{
			await Task.CompletedTask;

			const string text = "---...";

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

			Assert.Equal(5, parsingEvents.Count);
			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);
			Assert.True(parsingEvents[2] is Scalar);
			Assert.True(parsingEvents[3] is DocumentEnd);
			Assert.True(parsingEvents[4] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_DocumentStartAndDocumentEndWithNewLineBetween_ShouldResultInAStreamStartAndADocumentStartAndAScalarAndADocumentEndAndAStreamEnd()
		{
			await Task.CompletedTask;

			var text = $"---{Environment.NewLine}...";

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

			Assert.Equal(5, parsingEvents.Count);
			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);
			Assert.True(parsingEvents[2] is Scalar);
			Assert.True(parsingEvents[3] is DocumentEnd);
			Assert.True(parsingEvents[4] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_DocumentStartOnly_ShouldResultInAStreamStartAndADocumentStartAndAScalarAndADocumentEndAndAStreamEnd()
		{
			await Task.CompletedTask;

			const string text = "---";

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

			Assert.Equal(5, parsingEvents.Count);
			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);
			Assert.True(parsingEvents[2] is Scalar);
			Assert.True(parsingEvents[3] is DocumentEnd);
			Assert.True(parsingEvents[4] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_Empty_ShouldResultInAStreamStartAndAStreamEnd()
		{
			await Task.CompletedTask;

			const string text = "";

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
			Assert.True(parsingEvents[1] is StreamEnd);
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