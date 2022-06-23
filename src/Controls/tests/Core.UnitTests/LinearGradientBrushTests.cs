using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class LinearGradientBrushTests : BaseTestFixtureXUnit
	{
		
		public override void Setup()
		{
			
		}

		[Fact]
		public void TestConstructor()
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

			Assert.Equal(1.0d, linearGradientBrush.EndPoint.X, "EndPoint.X");
			Assert.Equal(1.0d, linearGradientBrush.EndPoint.Y, "EndPoint.Y");
		}

		[Fact]
		public void TestConstructorUsingGradientStopCollection()
		{
			var gradientStops = new GradientStopCollection
			{
				new GradientStop { Color = Colors.Red, Offset = 0.1f },
				new GradientStop { Color = Colors.Orange, Offset = 0.8f }
			};

			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStops, new Point(0, 0), new Point(0, 1));

			Assert.AreNotEqual(0, linearGradientBrush.GradientStops.Count, "GradientStops");
			Assert.Equal(0.0d, linearGradientBrush.EndPoint.X, "EndPoint.X");
			Assert.Equal(1.0d, linearGradientBrush.EndPoint.Y, "EndPoint.Y");
		}

		[Fact]
		public void TestEmptyLinearGradientBrush()
		{
			LinearGradientBrush nullLinearGradientBrush = new LinearGradientBrush();
			Assert.Equal(true, nullLinearGradientBrush.IsEmpty, "IsEmpty");

			LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0),
				GradientStops = new GradientStopCollection
				{
					new GradientStop { Color = Colors.Orange, Offset = 0.1f },
					new GradientStop { Color = Colors.Red, Offset = 0.8f }
				}
			};

			Assert.Equal(false, linearGradientBrush.IsEmpty, "IsEmpty");
		}

		[Fact]
		public void TestNullOrEmptyLinearGradientBrush()
		{
			LinearGradientBrush nullLinearGradientBrush = null;
			Assert.Equal(true, Brush.IsNullOrEmpty(nullLinearGradientBrush), "IsNullOrEmpty");

			LinearGradientBrush emptyLinearGradientBrush = new LinearGradientBrush();
			Assert.Equal(true, Brush.IsNullOrEmpty(emptyLinearGradientBrush), "IsNullOrEmpty");

			LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0),
				GradientStops = new GradientStopCollection
				{
					new GradientStop { Color = Colors.Orange, Offset = 0.1f },
					new GradientStop { Color = Colors.Red, Offset = 0.8f }
				}
			};

			Assert.Equal(false, Brush.IsNullOrEmpty(linearGradientBrush), "IsNullOrEmpty");
		}

		[Fact]
		public void TestLinearGradientBrushPoints()
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0)
			};

			Assert.Equal(0, linearGradientBrush.StartPoint.X);
			Assert.Equal(0, linearGradientBrush.StartPoint.Y);

			Assert.Equal(1, linearGradientBrush.EndPoint.X);
			Assert.Equal(0, linearGradientBrush.EndPoint.Y);
		}

		[Fact]
		public void TestLinearGradientBrushOnlyOneGradientStop()
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				GradientStops = new GradientStopCollection
				{
					new GradientStop { Color = Colors.Red, }
				},
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0)
			};

			Assert.IsNotNull(linearGradientBrush);
		}

		[Fact]
		public void TestLinearGradientBrushGradientStops()
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				GradientStops = new GradientStopCollection
				{
					new GradientStop { Color = Colors.Red, Offset = 0.1f },
					new GradientStop { Color = Colors.Blue, Offset = 1.0f }
				},
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0)
			};

			Assert.Equal(2, linearGradientBrush.GradientStops.Count);
		}
	}
}