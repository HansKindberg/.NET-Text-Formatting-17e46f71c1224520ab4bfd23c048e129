using Shared.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace YamlDotNetTests.Core
{
	public class ScannerTest
	{
		#region Fields

		private static readonly DirectoryInfo _resourcesDirectory = new(Path.Combine(Global.ProjectDirectory.FullName, "Core", "Resources", "ScannerTest"));

		#endregion

		#region Methods

		[Theory]
		[InlineData("BlockEntries-01", 3)]
		[InlineData("BlockEntries-02", 0)]
		public async Task BlockEntries_Test(string fileName, int expectedNumberOfBlockEntries)
		{
			await Task.CompletedTask;

			var tokens = await GetTokens(fileName);

			Assert.Equal(expectedNumberOfBlockEntries, tokens.Count(token => token is BlockEntry));
		}

		[Theory]
		[InlineData("BlockSequence-01", 10, 1, 0, 1, 3, 0, 0)]
		[InlineData("BlockSequence-02", 13, 0, 1, 1, 3, 0, 0)]
		[InlineData("BlockSequence-03", 10, 1, 0, 1, 3, 0, 0)]
		[InlineData("BlockSequence-04", 15, 1, 1, 2, 3, 0, 0)]
		[InlineData("BlockSequence-05", 24, 1, 2, 3, 3, 0, 0)]
		[InlineData("BlockSequence-06", 29, 1, 3, 4, 3, 0, 0)]
		[InlineData("BlockSequence-07", 24, 1, 2, 3, 3, 0, 0)]
		[InlineData("BlockSequence-08", 27, 0, 3, 3, 3, 0, 0)]
		[InlineData("BlockSequence-09", 10, 1, 0, 1, 3, 0, 0)]
		[InlineData("BlockSequence-10", 13, 0, 1, 1, 3, 0, 0)]
		[InlineData("BlockSequence-11", 10, 1, 0, 1, 3, 0, 0)]
		[InlineData("BlockSequence-12", 15, 1, 1, 2, 3, 0, 0)]
		[InlineData("BlockSequence-13", 24, 1, 2, 3, 3, 0, 0)]
		[InlineData("BlockSequence-14", 29, 1, 3, 4, 3, 0, 0)]
		[InlineData("BlockSequence-15", 24, 1, 2, 3, 3, 0, 0)]
		[InlineData("BlockSequence-16", 27, 0, 3, 3, 3, 0, 0)]
		[InlineData("BlockSequence-17", 12, 1, 0, 1, 3, 2, 1)]
		[InlineData("BlockSequence-18", 15, 0, 1, 1, 3, 2, 1)]
		[InlineData("BlockSequence-19", 12, 1, 0, 1, 3, 2, 1)]
		[InlineData("BlockSequence-20", 17, 1, 1, 2, 3, 2, 1)]
		[InlineData("BlockSequence-21", 26, 1, 2, 3, 3, 2, 1)]
		[InlineData("BlockSequence-22", 31, 1, 3, 4, 3, 2, 2)]
		[InlineData("BlockSequence-23", 26, 1, 2, 3, 3, 2, 2)]
		[InlineData("BlockSequence-24", 29, 0, 3, 3, 3, 2, 1)]
		public async Task BlockSequence_Test(string fileName, int expectedNumberOfTokens, int expectedNumberOfBlockSequenceStart, int expectedNumberOfBlockMappingStart, int expectedNumberOfBlockEnd, int expectedNumberOfBlockEntry, int expectedNumberOfComments, int expectedNumberOfInlineComments)
		{
			await Task.CompletedTask;

			var tokens = await GetTokens(fileName);

			Assert.Equal(expectedNumberOfTokens, tokens.Count);
			Assert.Equal(expectedNumberOfBlockSequenceStart, tokens.Count(token => token is BlockSequenceStart));
			Assert.Equal(expectedNumberOfBlockMappingStart, tokens.Count(token => token is BlockMappingStart));
			Assert.Equal(expectedNumberOfBlockEnd, tokens.Count(token => token is BlockEnd));
			Assert.Equal(expectedNumberOfBlockEntry, tokens.Count(token => token is BlockEntry));
			Assert.Equal(expectedNumberOfComments, tokens.Count(token => token is Comment));
			Assert.Equal(expectedNumberOfInlineComments, tokens.Count(token => token is Comment { IsInline: true }));
		}

		[Theory]
		[InlineData("FlowEntries-01", 2)]
		[InlineData("FlowEntries-02", 2)]
		[InlineData("FlowEntries-03", 2)]
		public async Task FlowEntries_Test(string fileName, int expectedNumberOfFlowEntries)
		{
			await Task.CompletedTask;

			var tokens = await GetTokens(fileName);

			Assert.Equal(expectedNumberOfFlowEntries, tokens.Count(token => token is FlowEntry));
		}

		private static async Task<string> GetFileText(string path)
		{
			await Task.CompletedTask;

			// ReSharper disable MethodHasAsyncOverload
			return File.ReadAllText(path).ResolveNewLine();
			// ReSharper restore MethodHasAsyncOverload
		}

		private static async Task<IList<Token>> GetTokens(string fileName)
		{
			var tokens = new List<Token>();

			using(var stringReader = new StringReader(await GetYaml(fileName)))
			{
				var scanner = new Scanner(stringReader, false);

				while(scanner.MoveNext())
				{
					tokens.Add(scanner.Current!);
				}
			}

			return tokens;
		}

		private static async Task<string> GetYaml(string fileName)
		{
			return await GetFileText(Path.Combine(_resourcesDirectory.FullName, $"{fileName}.yml"));
		}

		#endregion
	}
}