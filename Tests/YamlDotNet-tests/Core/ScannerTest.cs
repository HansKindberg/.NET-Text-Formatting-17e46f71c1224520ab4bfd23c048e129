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
		[InlineData("FlowEntries-01", 2)]
		[InlineData("FlowEntries-02", 2)]
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