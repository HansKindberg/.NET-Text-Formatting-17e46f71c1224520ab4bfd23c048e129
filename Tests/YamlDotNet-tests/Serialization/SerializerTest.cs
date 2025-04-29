using System.Security.Claims;
using Shared.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNetTests.Serialization
{
	public class SerializerTest
	{
		#region Methods

		private static async Task<INamingConvention?> GetNamingConvention(Type? namingConventionType)
		{
			await Task.CompletedTask;

			if(namingConventionType == null)
				return null;

			if(namingConventionType == typeof(CamelCaseNamingConvention))
				return CamelCaseNamingConvention.Instance;

			if(namingConventionType == typeof(HyphenatedNamingConvention))
				return HyphenatedNamingConvention.Instance;

			if(namingConventionType == typeof(LowerCaseNamingConvention))
				return LowerCaseNamingConvention.Instance;

			if(namingConventionType == typeof(PascalCaseNamingConvention))
				return PascalCaseNamingConvention.Instance;

			if(namingConventionType == typeof(UnderscoredNamingConvention))
				return UnderscoredNamingConvention.Instance;

			if(namingConventionType == typeof(NullNamingConvention))
				return NullNamingConvention.Instance;

			throw new ArgumentOutOfRangeException(nameof(namingConventionType), namingConventionType, "The specified naming convention is not supported.");
		}

		private static async Task<ISerializer> GetSerializer(Type? namingConventionType)
		{
			var serializerBuilder = new SerializerBuilder();

			var namingConvention = await GetNamingConvention(namingConventionType);

			if(namingConvention != null)
				serializerBuilder = serializerBuilder.WithNamingConvention(namingConvention);

			return serializerBuilder.Build();
		}

		[Theory]
		[InlineData("- Issuer: firstIssuer\n  OriginalIssuer: firstOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: firstType\n  Value: firstValue\n  ValueType: firstValueType\n- Issuer: secondIssuer\n  OriginalIssuer: secondOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: secondType\n  Value: secondValue\n  ValueType: secondValueType\n- Issuer: thirdIssuer\n  OriginalIssuer: thirdOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: thirdType\n  Value: thirdValue\n  ValueType: thirdValueType\n", null)]
		[InlineData("- issuer: firstIssuer\n  originalIssuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  valueType: firstValueType\n- issuer: secondIssuer\n  originalIssuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  valueType: secondValueType\n- issuer: thirdIssuer\n  originalIssuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  valueType: thirdValueType\n", typeof(CamelCaseNamingConvention))]
		[InlineData("- issuer: firstIssuer\n  original-issuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  value-type: firstValueType\n- issuer: secondIssuer\n  original-issuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  value-type: secondValueType\n- issuer: thirdIssuer\n  original-issuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  value-type: thirdValueType\n", typeof(HyphenatedNamingConvention))]
		[InlineData("- issuer: firstIssuer\n  originalissuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  valuetype: firstValueType\n- issuer: secondIssuer\n  originalissuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  valuetype: secondValueType\n- issuer: thirdIssuer\n  originalissuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  valuetype: thirdValueType\n", typeof(LowerCaseNamingConvention))]
		[InlineData("- Issuer: firstIssuer\n  OriginalIssuer: firstOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: firstType\n  Value: firstValue\n  ValueType: firstValueType\n- Issuer: secondIssuer\n  OriginalIssuer: secondOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: secondType\n  Value: secondValue\n  ValueType: secondValueType\n- Issuer: thirdIssuer\n  OriginalIssuer: thirdOriginalIssuer\n  Properties: {}\n  Subject: \n  Type: thirdType\n  Value: thirdValue\n  ValueType: thirdValueType\n", typeof(PascalCaseNamingConvention))]
		[InlineData("- issuer: firstIssuer\n  original_issuer: firstOriginalIssuer\n  properties: {}\n  subject: \n  type: firstType\n  value: firstValue\n  value_type: firstValueType\n- issuer: secondIssuer\n  original_issuer: secondOriginalIssuer\n  properties: {}\n  subject: \n  type: secondType\n  value: secondValue\n  value_type: secondValueType\n- issuer: thirdIssuer\n  original_issuer: thirdOriginalIssuer\n  properties: {}\n  subject: \n  type: thirdType\n  value: thirdValue\n  value_type: thirdValueType\n", typeof(UnderscoredNamingConvention))]
		public async Task Serialize_ClaimList_ShouldWorkProperly(string expected, Type? namingConventionType)
		{
			await Task.CompletedTask;

			var list = new List<Claim>
			{
				new("firstType", "firstValue", "firstValueType", "firstIssuer", "firstOriginalIssuer"),
				new("secondType", "secondValue", "secondValueType", "secondIssuer", "secondOriginalIssuer"),
				new("thirdType", "thirdValue", "thirdValueType", "thirdIssuer", "thirdOriginalIssuer"),
			};

			var serializer = await GetSerializer(namingConventionType);

			var yaml = serializer.Serialize(list);

			Assert.Equal(expected.ResolveNewLine(), yaml);
		}

		[Theory]
		[InlineData("- a\n- B\n- c\n- D\n", null)]
		[InlineData("- a\n- B\n- c\n- D\n", typeof(CamelCaseNamingConvention))]
		[InlineData("- a\n- B\n- c\n- D\n", typeof(HyphenatedNamingConvention))]
		[InlineData("- a\n- B\n- c\n- D\n", typeof(LowerCaseNamingConvention))]
		[InlineData("- a\n- B\n- c\n- D\n", typeof(PascalCaseNamingConvention))]
		[InlineData("- a\n- B\n- c\n- D\n", typeof(UnderscoredNamingConvention))]
		public async Task Serialize_StringList_ShouldWorkProperly(string expected, Type? namingConventionType)
		{
			var list = new List<string> { "a", "B", "c", "D" };

			var serializer = await GetSerializer(namingConventionType);

			var yaml = serializer.Serialize(list);

			Assert.Equal(expected.ResolveNewLine(), yaml);
		}

		#endregion
	}
}