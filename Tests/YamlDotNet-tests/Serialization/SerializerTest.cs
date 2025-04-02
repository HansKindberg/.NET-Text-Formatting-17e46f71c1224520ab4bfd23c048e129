using System.Security.Claims;
using HansKindberg.Text.Formatting.Yaml.Serialization;
using Shared.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNetTests.Serialization
{
	public class SerializerTest
	{
		#region Methods

		private static async Task<INamingConvention> GetNamingConvention(NamingConvention? namingConvention)
		{
			await Task.CompletedTask;

			return namingConvention switch
			{
				NamingConvention.CamelCase => CamelCaseNamingConvention.Instance,
				NamingConvention.Hyphenated => HyphenatedNamingConvention.Instance,
				NamingConvention.LowerCase => LowerCaseNamingConvention.Instance,
				NamingConvention.PascalCase => PascalCaseNamingConvention.Instance,
				NamingConvention.Underscored => UnderscoredNamingConvention.Instance,
				_ => NullNamingConvention.Instance
			};
		}

		private static async Task<ISerializer> GetSerializer(NamingConvention? namingConvention)
		{
			return new SerializerBuilder()
				.WithNamingConvention(await GetNamingConvention(namingConvention))
				.Build();
		}

		[Theory]
		[InlineData("- Issuer: firstIssuer\n  OriginalIssuer: firstOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: firstType\n  Value: firstValue\n  ValueType: firstValueType\n- Issuer: secondIssuer\n  OriginalIssuer: secondOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: secondType\n  Value: secondValue\n  ValueType: secondValueType\n- Issuer: thirdIssuer\n  OriginalIssuer: thirdOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: thirdType\n  Value: thirdValue\n  ValueType: thirdValueType\n", null)]
		[InlineData("- issuer: firstIssuer\n  originalIssuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  valueType: firstValueType\n- issuer: secondIssuer\n  originalIssuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  valueType: secondValueType\n- issuer: thirdIssuer\n  originalIssuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  valueType: thirdValueType\n", NamingConvention.CamelCase)]
		[InlineData("- issuer: firstIssuer\n  original-issuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  value-type: firstValueType\n- issuer: secondIssuer\n  original-issuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  value-type: secondValueType\n- issuer: thirdIssuer\n  original-issuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  value-type: thirdValueType\n", NamingConvention.Hyphenated)]
		[InlineData("- issuer: firstIssuer\n  originalissuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  valuetype: firstValueType\n- issuer: secondIssuer\n  originalissuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  valuetype: secondValueType\n- issuer: thirdIssuer\n  originalissuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  valuetype: thirdValueType\n", NamingConvention.LowerCase)]
		[InlineData("- Issuer: firstIssuer\n  OriginalIssuer: firstOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: firstType\n  Value: firstValue\n  ValueType: firstValueType\n- Issuer: secondIssuer\n  OriginalIssuer: secondOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: secondType\n  Value: secondValue\n  ValueType: secondValueType\n- Issuer: thirdIssuer\n  OriginalIssuer: thirdOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: thirdType\n  Value: thirdValue\n  ValueType: thirdValueType\n", NamingConvention.PascalCase)]
		[InlineData("- issuer: firstIssuer\n  original_issuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  value_type: firstValueType\n- issuer: secondIssuer\n  original_issuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  value_type: secondValueType\n- issuer: thirdIssuer\n  original_issuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  value_type: thirdValueType\n", NamingConvention.Underscored)]
		public async Task Serialize_ClaimList_ShouldWorkProperly(string expected, NamingConvention? namingConvention)
		{
			await Task.CompletedTask;

			var list = new List<Claim>
			{
				new("firstType", "firstValue", "firstValueType", "firstIssuer", "firstOriginalIssuer"),
				new("secondType", "secondValue", "secondValueType", "secondIssuer", "secondOriginalIssuer"),
				new("thirdType", "thirdValue", "thirdValueType", "thirdIssuer", "thirdOriginalIssuer"),
			};

			var serializer = await GetSerializer(namingConvention);

			var yaml = serializer.Serialize(list);

			Assert.Equal(expected.ResolveNewLine(), yaml);
		}

		[Theory]
		[InlineData("- a\n- B\n- c\n- D\n", null)]
		[InlineData("- a\n- B\n- c\n- D\n", NamingConvention.CamelCase)]
		[InlineData("- a\n- B\n- c\n- D\n", NamingConvention.Hyphenated)]
		[InlineData("- a\n- B\n- c\n- D\n", NamingConvention.LowerCase)]
		[InlineData("- a\n- B\n- c\n- D\n", NamingConvention.PascalCase)]
		[InlineData("- a\n- B\n- c\n- D\n", NamingConvention.Underscored)]
		public async Task Serialize_StringList_ShouldWorkProperly(string expected, NamingConvention? namingConvention)
		{
			var list = new List<string> { "a", "B", "c", "D" };

			var serializer = await GetSerializer(namingConvention);

			var yaml = serializer.Serialize(list);

			Assert.Equal(expected.ResolveNewLine(), yaml);
		}

		#endregion
	}
}