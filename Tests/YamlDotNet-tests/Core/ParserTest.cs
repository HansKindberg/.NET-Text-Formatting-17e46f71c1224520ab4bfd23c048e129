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

		[Fact]
		public async Task Current_Iteration_CommentsOnly_ShouldResultInAStreamStartAndTheFirstCommentOnly()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Comments-Only");

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
			Assert.False(((Comment)parsingEvents[1]).IsInline);
			Assert.Equal("First comment", ((Comment)parsingEvents[1]).Value);
		}

		[Fact]
		public async Task Current_Iteration_CommentsOnlyWithLeadingDocumentStart_ShouldResultInParsingEventsForAllComments()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Comments-Only-WithLeadingDocumentStart");

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

			Assert.Equal(19, parsingEvents.Count);

			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);

			Assert.True(parsingEvents[2] is Comment);
			Assert.False(((Comment)parsingEvents[2]).IsInline);
			Assert.Equal("First comment", ((Comment)parsingEvents[2]).Value);

			Assert.True(parsingEvents[3] is Comment);
			Assert.False(((Comment)parsingEvents[3]).IsInline);
			Assert.Equal("Second comment", ((Comment)parsingEvents[3]).Value);

			Assert.True(parsingEvents[4] is Comment);
			Assert.False(((Comment)parsingEvents[4]).IsInline);
			Assert.Equal("Third comment", ((Comment)parsingEvents[4]).Value);

			Assert.True(parsingEvents[5] is Comment);
			Assert.False(((Comment)parsingEvents[5]).IsInline);
			Assert.Equal("Fourth comment", ((Comment)parsingEvents[5]).Value);

			Assert.True(parsingEvents[6] is Comment);
			Assert.False(((Comment)parsingEvents[6]).IsInline);
			Assert.Equal("Fifth comment", ((Comment)parsingEvents[6]).Value);

			Assert.True(parsingEvents[7] is Comment);
			Assert.False(((Comment)parsingEvents[7]).IsInline);
			Assert.Equal("Sixth comment", ((Comment)parsingEvents[7]).Value);

			Assert.True(parsingEvents[8] is Comment);
			Assert.False(((Comment)parsingEvents[8]).IsInline);
			Assert.Equal("Seventh comment", ((Comment)parsingEvents[8]).Value);

			Assert.True(parsingEvents[9] is Comment);
			Assert.False(((Comment)parsingEvents[9]).IsInline);
			Assert.Equal("Eighth comment", ((Comment)parsingEvents[9]).Value);

			Assert.True(parsingEvents[10] is Comment);
			Assert.False(((Comment)parsingEvents[10]).IsInline);
			Assert.Equal("Ninth comment", ((Comment)parsingEvents[10]).Value);

			Assert.True(parsingEvents[11] is Comment);
			Assert.False(((Comment)parsingEvents[11]).IsInline);
			Assert.Equal("Tenth comment", ((Comment)parsingEvents[11]).Value);

			Assert.True(parsingEvents[12] is Comment);
			Assert.False(((Comment)parsingEvents[12]).IsInline);
			Assert.Equal("Eleventh comment", ((Comment)parsingEvents[12]).Value);

			Assert.True(parsingEvents[13] is Comment);
			Assert.False(((Comment)parsingEvents[13]).IsInline);
			Assert.Equal("Twelfth comment", ((Comment)parsingEvents[13]).Value);

			Assert.True(parsingEvents[14] is Comment);
			Assert.False(((Comment)parsingEvents[14]).IsInline);
			Assert.Equal("Thirteenth comment", ((Comment)parsingEvents[14]).Value);

			Assert.True(parsingEvents[15] is Comment);
			Assert.False(((Comment)parsingEvents[15]).IsInline);
			Assert.Equal("Fourteenth comment", ((Comment)parsingEvents[15]).Value);

			Assert.True(parsingEvents[16] is Scalar);
			Assert.Empty(((Scalar)parsingEvents[16]).Value);
			Assert.False(((Scalar)parsingEvents[16]).IsKey);
			Assert.Equal(ScalarStyle.Plain, ((Scalar)parsingEvents[16]).Style);
			Assert.Equal(((Scalar)parsingEvents[16]).End.Column, ((Scalar)parsingEvents[16]).Start.Column);
			Assert.Equal(((Scalar)parsingEvents[16]).End.Index, ((Scalar)parsingEvents[16]).Start.Index);
			Assert.Equal(((Scalar)parsingEvents[16]).End.Line, ((Scalar)parsingEvents[16]).Start.Line);
			Assert.True(((Scalar)parsingEvents[16]).Tag.IsEmpty);

			Assert.True(parsingEvents[17] is DocumentEnd);
			Assert.True(parsingEvents[18] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_CommentsOnlyWithLeadingDocumentStartWithComment_ShouldResultInParsingEventsForAllComments()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Comments-Only-WithLeadingDocumentStartWithComment");

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

			Assert.Equal(20, parsingEvents.Count);

			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);

			Assert.True(parsingEvents[2] is Comment);
			Assert.True(((Comment)parsingEvents[2]).IsInline);
			Assert.Equal("Leading document comment", ((Comment)parsingEvents[2]).Value);

			Assert.True(parsingEvents[3] is Comment);
			Assert.False(((Comment)parsingEvents[3]).IsInline);
			Assert.Equal("First comment", ((Comment)parsingEvents[3]).Value);

			Assert.True(parsingEvents[4] is Comment);
			Assert.False(((Comment)parsingEvents[4]).IsInline);
			Assert.Equal("Second comment", ((Comment)parsingEvents[4]).Value);

			Assert.True(parsingEvents[5] is Comment);
			Assert.False(((Comment)parsingEvents[5]).IsInline);
			Assert.Equal("Third comment", ((Comment)parsingEvents[5]).Value);

			Assert.True(parsingEvents[6] is Comment);
			Assert.False(((Comment)parsingEvents[6]).IsInline);
			Assert.Equal("Fourth comment", ((Comment)parsingEvents[6]).Value);

			Assert.True(parsingEvents[7] is Comment);
			Assert.False(((Comment)parsingEvents[7]).IsInline);
			Assert.Equal("Fifth comment", ((Comment)parsingEvents[7]).Value);

			Assert.True(parsingEvents[8] is Comment);
			Assert.False(((Comment)parsingEvents[8]).IsInline);
			Assert.Equal("Sixth comment", ((Comment)parsingEvents[8]).Value);

			Assert.True(parsingEvents[9] is Comment);
			Assert.False(((Comment)parsingEvents[9]).IsInline);
			Assert.Equal("Seventh comment", ((Comment)parsingEvents[9]).Value);

			Assert.True(parsingEvents[10] is Comment);
			Assert.False(((Comment)parsingEvents[10]).IsInline);
			Assert.Equal("Eighth comment", ((Comment)parsingEvents[10]).Value);

			Assert.True(parsingEvents[11] is Comment);
			Assert.False(((Comment)parsingEvents[11]).IsInline);
			Assert.Equal("Ninth comment", ((Comment)parsingEvents[11]).Value);

			Assert.True(parsingEvents[12] is Comment);
			Assert.False(((Comment)parsingEvents[12]).IsInline);
			Assert.Equal("Tenth comment", ((Comment)parsingEvents[12]).Value);

			Assert.True(parsingEvents[13] is Comment);
			Assert.False(((Comment)parsingEvents[13]).IsInline);
			Assert.Equal("Eleventh comment", ((Comment)parsingEvents[13]).Value);

			Assert.True(parsingEvents[14] is Comment);
			Assert.False(((Comment)parsingEvents[14]).IsInline);
			Assert.Equal("Twelfth comment", ((Comment)parsingEvents[14]).Value);

			Assert.True(parsingEvents[15] is Comment);
			Assert.False(((Comment)parsingEvents[15]).IsInline);
			Assert.Equal("Thirteenth comment", ((Comment)parsingEvents[15]).Value);

			Assert.True(parsingEvents[16] is Comment);
			Assert.False(((Comment)parsingEvents[16]).IsInline);
			Assert.Equal("Fourteenth comment", ((Comment)parsingEvents[16]).Value);

			Assert.True(parsingEvents[17] is Scalar);
			Assert.Empty(((Scalar)parsingEvents[17]).Value);
			Assert.False(((Scalar)parsingEvents[17]).IsKey);
			Assert.Equal(ScalarStyle.Plain, ((Scalar)parsingEvents[17]).Style);
			Assert.Equal(((Scalar)parsingEvents[17]).End.Column, ((Scalar)parsingEvents[17]).Start.Column);
			Assert.Equal(((Scalar)parsingEvents[17]).End.Index, ((Scalar)parsingEvents[17]).Start.Index);
			Assert.Equal(((Scalar)parsingEvents[17]).End.Line, ((Scalar)parsingEvents[17]).Start.Line);
			Assert.True(((Scalar)parsingEvents[17]).Tag.IsEmpty);

			Assert.True(parsingEvents[18] is DocumentEnd);
			Assert.True(parsingEvents[19] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_CommentsOnlyWithLeadingScalarKey_ShouldResultInParsingEventsForAllComments()
		{
			await Task.CompletedTask;

			var text = await GetYaml("Comments-Only-WithLeadingScalarKey");

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

			Assert.True(parsingEvents[0] is StreamStart);
			Assert.True(parsingEvents[1] is DocumentStart);
			Assert.True(parsingEvents[2] is MappingStart);

			Assert.True(parsingEvents[3] is Scalar);
			Assert.True(((Scalar)parsingEvents[3]).IsKey);
			Assert.Equal("a", ((Scalar)parsingEvents[3]).Value);

			Assert.True(parsingEvents[4] is Comment);
			Assert.False(((Comment)parsingEvents[4]).IsInline);
			Assert.Equal("First comment", ((Comment)parsingEvents[4]).Value);

			Assert.True(parsingEvents[5] is Comment);
			Assert.False(((Comment)parsingEvents[5]).IsInline);
			Assert.Equal("Second comment", ((Comment)parsingEvents[5]).Value);

			Assert.True(parsingEvents[6] is Comment);
			Assert.False(((Comment)parsingEvents[6]).IsInline);
			Assert.Equal("Third comment", ((Comment)parsingEvents[6]).Value);

			Assert.True(parsingEvents[7] is Comment);
			Assert.False(((Comment)parsingEvents[7]).IsInline);
			Assert.Equal("Fourth comment", ((Comment)parsingEvents[7]).Value);

			Assert.True(parsingEvents[8] is Comment);
			Assert.False(((Comment)parsingEvents[8]).IsInline);
			Assert.Equal("Fifth comment", ((Comment)parsingEvents[8]).Value);

			Assert.True(parsingEvents[9] is Comment);
			Assert.False(((Comment)parsingEvents[9]).IsInline);
			Assert.Equal("Sixth comment", ((Comment)parsingEvents[9]).Value);

			Assert.True(parsingEvents[10] is Comment);
			Assert.False(((Comment)parsingEvents[10]).IsInline);
			Assert.Equal("Seventh comment", ((Comment)parsingEvents[10]).Value);

			Assert.True(parsingEvents[11] is Comment);
			Assert.False(((Comment)parsingEvents[11]).IsInline);
			Assert.Equal("Eighth comment", ((Comment)parsingEvents[11]).Value);

			Assert.True(parsingEvents[12] is Comment);
			Assert.False(((Comment)parsingEvents[12]).IsInline);
			Assert.Equal("Ninth comment", ((Comment)parsingEvents[12]).Value);

			Assert.True(parsingEvents[13] is Comment);
			Assert.False(((Comment)parsingEvents[13]).IsInline);
			Assert.Equal("Tenth comment", ((Comment)parsingEvents[13]).Value);

			Assert.True(parsingEvents[14] is Comment);
			Assert.False(((Comment)parsingEvents[14]).IsInline);
			Assert.Equal("Eleventh comment", ((Comment)parsingEvents[14]).Value);

			Assert.True(parsingEvents[15] is Comment);
			Assert.False(((Comment)parsingEvents[15]).IsInline);
			Assert.Equal("Twelfth comment", ((Comment)parsingEvents[15]).Value);

			Assert.True(parsingEvents[16] is Comment);
			Assert.False(((Comment)parsingEvents[16]).IsInline);
			Assert.Equal("Thirteenth comment", ((Comment)parsingEvents[16]).Value);

			Assert.True(parsingEvents[17] is Comment);
			Assert.False(((Comment)parsingEvents[17]).IsInline);
			Assert.Equal("Fourteenth comment", ((Comment)parsingEvents[17]).Value);

			Assert.True(parsingEvents[18] is Scalar);
			Assert.Empty(((Scalar)parsingEvents[18]).Value);
			Assert.False(((Scalar)parsingEvents[18]).IsKey);
			Assert.Equal(ScalarStyle.Plain, ((Scalar)parsingEvents[18]).Style);
			Assert.Equal(((Scalar)parsingEvents[18]).End.Column, ((Scalar)parsingEvents[18]).Start.Column);
			Assert.Equal(((Scalar)parsingEvents[18]).End.Index, ((Scalar)parsingEvents[18]).Start.Index);
			Assert.Equal(((Scalar)parsingEvents[18]).End.Line, ((Scalar)parsingEvents[18]).Start.Line);
			Assert.True(((Scalar)parsingEvents[18]).Tag.IsEmpty);

			Assert.True(parsingEvents[19] is MappingEnd);
			Assert.True(parsingEvents[20] is DocumentEnd);
			Assert.True(parsingEvents[21] is StreamEnd);
		}

		[Fact]
		public async Task Current_Iteration_DocumentEndFollowedByDocumentStart_ShouldThrowAnInvalidOperationException()
		{
			var text = await GetYaml("DocumentEnd-FollowedBy-DocumentStart");

			var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
			{
				using(var stringReader = new StringReader(text))
				{
					var scanner = new Scanner(stringReader, false);
					var parser = new Parser(scanner);

					while(parser.MoveNext()) { }
				}

				return Task.CompletedTask;
			});

			Assert.NotNull(invalidOperationException);
			Assert.Equal("The scanner should contain no more tokens.", invalidOperationException.Message);
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

		[Fact]
		public async Task Current_Iteration_InlineInvalidMapping_ShouldWorkProperly()
		{
			await Task.CompletedTask;
			const string text = "key: {value-1, value-2}";
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

			Assert.Equal(13, parsingEvents.Count);
		}

		[Fact]
		public async Task Current_Iteration_InlineMapping_ShouldWorkProperly()
		{
			await Task.CompletedTask;
			const string text = "key: {value-1: null, value-2: null}";
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

			Assert.Equal(13, parsingEvents.Count);
		}

		[Fact]
		public async Task Current_Iteration_InlineSequence_ShouldWorkProperly()
		{
			await Task.CompletedTask;
			const string text = "key: [value-1, value-2]";
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

			Assert.Equal(11, parsingEvents.Count);
			Assert.Single(parsingEvents.OfType<SequenceEnd>());
			Assert.Single(parsingEvents.OfType<SequenceStart>());
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