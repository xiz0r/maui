using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class FontAttributeConverterUnitTests : BaseTestFixtureXUnit
	{
		[TestCase("None", FontAttributes.None)]
		[TestCase("Bold", FontAttributes.Bold)]
		[TestCase("italic", FontAttributes.Italic)]
		[TestCase("oblique", FontAttributes.Italic)]
		[TestCase("oblique, bold", FontAttributes.Italic | FontAttributes.Bold)]
		[TestCase("bold italic", FontAttributes.Italic | FontAttributes.Bold)]
		public void TestFontAttributeConverter(string input, FontAttributes expected)
		{
			var conv = new FontAttributesConverter();
			Assert.That(conv.ConvertFromInvariantString(input), Is.EqualTo(expected));
		}
	}
}