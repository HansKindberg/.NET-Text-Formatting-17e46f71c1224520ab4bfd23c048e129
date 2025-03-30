using HansKindberg.Text.Formatting.Json;
using Shared.Extensions;

namespace UnitTests.Json
{
	public class JsonParserTest
	{
		#region Methods

		[Fact]
		public async Task Parse_IfTheValueParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(async () => await new JsonParser().Parse(null!));
			Assert.Equal("value", argumentNullException.ParamName);
		}

		[Theory]
		[InlineData("{}", "{}")]
		[InlineData("     {}     ", "{}")]
		[InlineData("{     }", "{     }")]
		[InlineData("     {     }     ", "{     }")]
		[InlineData("         \n       {}  \n          \t    ", "{}")]
		[InlineData("         \n       {     \n     \n     \t     }  \n          \t    ", "{     \n     \n     \t     }")]
		[InlineData("{ \"First-property\":        \"First-value\"}", "{ \"First-property\":        \"First-value\"}")]
		[InlineData("[]", "[]")]
		[InlineData("     []     ", "[]")]
		[InlineData("[     ]", "[     ]")]
		[InlineData("     [     ]     ", "[     ]")]
		[InlineData("         \n       []  \n          \t    ", "[]")]
		[InlineData("         \n       [     \n     \n     \t     ]  \n          \t    ", "[     \n     \n     \t     ]")]
		[InlineData("[{ \"First-property\":        \"First-value\"}]", "[{ \"First-property\":        \"First-value\"}]")]
		public async Task Parse_ShouldWorkProperly(string value, string expected)
		{
			value = value.ResolveNewLine();
			expected = expected.ResolveNewLine();

			var jsonElement = await new JsonParser().Parse(value);
			Assert.Equal(expected, jsonElement.ToString());
		}

		#endregion
	}
}