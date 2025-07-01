using HansKindberg.Text.Formatting;
using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Models;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using Moq;
using YamlDotNet.Serialization.NamingConventions;

namespace UnitTests.Yaml
{
	public class YamlFormatterTest
	{
		#region Fields

		private static readonly YamlFormatter _yamlFormatter = new(Mock.Of<IParser<IYamlStream>>());

		#endregion

		#region Methods

		[Theory]
		[InlineData(null, null)]
		[InlineData(NamingConvention.CamelCase, typeof(CamelCaseNamingConvention))]
		[InlineData(NamingConvention.Hyphenated, typeof(HyphenatedNamingConvention))]
		[InlineData(NamingConvention.LowerCase, typeof(LowerCaseNamingConvention))]
		[InlineData(NamingConvention.PascalCase, typeof(PascalCaseNamingConvention))]
		[InlineData(NamingConvention.Underscored, typeof(UnderscoredNamingConvention))]
		public async Task GetNamingConvention_ShouldWorkProperly(NamingConvention? namingConvention, Type? expectedNamingConventionType)
		{
			var actualNamingConvention = await _yamlFormatter.GetNamingConvention(namingConvention);

			Assert.Equal(expectedNamingConventionType, actualNamingConvention?.GetType());
		}

		#endregion
	}
}