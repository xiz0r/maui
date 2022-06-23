using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class SolidColorBrushTests : BaseTestFixtureXUnit
	{
		
		public override void Setup()
		{
			
		}

		[Fact]
		public void TestConstructor()
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush();
			Assert.Null(solidColorBrush.Color, "Color");
		}

		[Fact]
		public void TestConstructorUsingColor()
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Red);
			Assert.That(solidColorBrush.Color, Is.EqualTo(Colors.Red));
		}

		[Fact]
		public void TestEmptySolidColorBrush()
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush();
			Assert.Equal(true, solidColorBrush.IsEmpty, "IsEmpty");

			SolidColorBrush red = Brush.Red;
			Assert.Equal(false, red.IsEmpty, "IsEmpty");
		}

		[Fact]
		public void TestNullOrEmptySolidColorBrush()
		{
			SolidColorBrush nullSolidColorBrush = null;
			Assert.Equal(true, Brush.IsNullOrEmpty(nullSolidColorBrush), "IsNullOrEmpty");

			SolidColorBrush emptySolidColorBrush = new SolidColorBrush();
			Assert.Equal(true, Brush.IsNullOrEmpty(emptySolidColorBrush), "IsNullOrEmpty");

			SolidColorBrush solidColorBrush = Brush.Yellow;
			Assert.Equal(false, Brush.IsNullOrEmpty(solidColorBrush), "IsNullOrEmpty");
		}

		[Fact]
		public void TestDefaultBrushes()
		{
			SolidColorBrush black = Brush.Black;
			Assert.IsNotNull(black.Color);
			Assert.That(black.Color, Is.EqualTo(Colors.Black));

			SolidColorBrush white = Brush.White;
			Assert.IsNotNull(white.Color);
			Assert.That(white.Color, Is.EqualTo(Colors.White));
		}
	}
}