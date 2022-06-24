using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class FontAttributeConverterUnitTests : BaseTestFixtureXUnit
	{
		[InlineData("None", FontAttributes.None)]
		[InlineData("Bold", FontAttributes.Bold)]
		[InlineData("italic", FontAttributes.Italic)]
		[InlineData("oblique", FontAttributes.Italic)]
		[InlineData("oblique, bold", FontAttributes.Italic | FontAttributes.Bold)]
		[InlineData("bold italic", FontAttributes.Italic | FontAttributes.Bold)]
		public void TestFontAttributeConverter(string input, FontAttributes expected)
		{
			var conv = new FontAttributesConverter();
			Assert.That(conv.ConvertFromInvariantString(input), Is.EqualTo(expected));
		}
	}
}