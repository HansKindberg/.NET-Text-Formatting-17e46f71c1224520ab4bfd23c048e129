using HansKindberg.Text.Formatting.Yaml;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using UnitTests.Helpers.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace UnitTests.Yaml
{
	public class ParsingEventStringifierTest
	{
		#region Fields

		private static readonly ParsingEventStringifier _parsingEventStringifier = new();


		//private static readonly IDictionary<Type, ParsingEvent> _parsingEventDictionary = new Dictionary<Type, ParsingEvent>
		//{
		//	{ typeof(AnchorAlias), new AnchorAlias("Test") },
		//	{ typeof(Comment), new Comment("Test", false)},
		//	{ typeof(DocumentEnd), new DocumentEnd(false) },
		//	{ typeof(DocumentStart), new DocumentStart(Mark.Empty, Mark.Empty) },
			
		//	{ typeof(Scalar), CreateScalar(false, ScalarStyle.Any, "Test") },
		//	{ typeof(SequenceStart), new SequenceStart(null, null, false, null) },
		//	{ typeof(SequenceEnd), new SequenceEnd() },
		//	{ typeof(MappingStart), new MappingStart(null, null, false, null) },
		//	{ typeof(MappingEnd), new MappingEnd() },


		//};

		#endregion

		#region Methods

		private static Scalar CreateScalar(bool isQuotedImplicit, ScalarStyle scalarStyle, string value)
		{
			return CreateScalar(null, null, null, isQuotedImplicit, scalarStyle, null, value);
		}

		private static Scalar CreateScalar(string? anchor, bool? isKey, bool? isPlainImplicit, bool? isQuotedImplicit, ScalarStyle? scalarStyle, string? tag, string? value)
		{
			var anchorName = string.IsNullOrEmpty(anchor) ? AnchorName.Empty : new AnchorName(anchor!);
			isKey ??= false;
			isPlainImplicit ??= false;
			isQuotedImplicit ??= false;
			scalarStyle ??= ScalarStyle.Plain;
			var tagName = string.IsNullOrEmpty(tag) ? TagName.Empty : new TagName(tag!);

			return new Scalar(anchorName, tagName, value ?? string.Empty, scalarStyle.Value, isPlainImplicit.Value, isQuotedImplicit.Value, Mark.Empty, Mark.Empty, isKey.Value);
		}

		[Theory]
		[InlineData(false, null, ScalarStyle.Any, "", "")]
		[InlineData(false, null, ScalarStyle.Any, "test", "test")]
		[InlineData(true, null, ScalarStyle.DoubleQuoted, "test", "\"test\"")]
		[InlineData(true, null, ScalarStyle.SingleQuoted, "test", "'test'")]
		[InlineData(true, Quotation.Clear, ScalarStyle.DoubleQuoted, "test", "test")]
		[InlineData(true, Quotation.Clear, ScalarStyle.SingleQuoted, "test", "test")]
		[InlineData(false, Quotation.Double, ScalarStyle.Any, "test", "\"test\"")]
		[InlineData(false, Quotation.Single, ScalarStyle.Any, "test", "'test'")]
		public async Task GetValue_ShouldWorkProperly(bool isQuotedImplicit, Quotation? quotation, ScalarStyle scalarStyle, string value, string? expectedValue)
		{
			await Task.CompletedTask;

			var scalar = CreateScalar(isQuotedImplicit, scalarStyle, value);
			var actualValue = await _parsingEventStringifier.GetValue(quotation, scalar);
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public async Task Stringify_IfOptionsAreNull_ShouldThrowAnArgumentNullException()
		{
			CultureHelper.SetTestCulture();

			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _parsingEventStringifier.Stringify(null!, null));
			Assert.NotNull(argumentNullException);
			Assert.Equal("options", argumentNullException.ParamName);
			Assert.StartsWith("Value cannot be null.", argumentNullException.Message);
		}

		[Fact]
		public async Task Stringify_IfParsingEventIsNull_ShouldReturnNull()
		{
			var value = await _parsingEventStringifier.Stringify(new YamlFormatOptions(), null);
			Assert.Null(value);
		}

		#endregion
	}
}