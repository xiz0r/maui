using System.Collections.Generic;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class GeometryTests : BaseTestFixtureXUnit
	{
		[TestCase(0, true)]
		[TestCase(0, false)]
		[TestCase(45, true)]
		[TestCase(45, false)]
		[TestCase(180, true)]
		[TestCase(180, false)]
		[TestCase(270, true)]
		[TestCase(270, false)]
		public void FlattenArcTest(double angle, bool isLargeArc)
		{
			var path = new Path
			{
				HeightRequest = 200,
				WidthRequest = 200,
				Stroke = Brush.Black
			};

			PathFigure figure = new PathFigure();

			ArcSegment arcSegment = new ArcSegment
			{
				Point = new Point(10, 100),
				Size = new Size(100, 50),
				RotationAngle = angle,
				IsLargeArc = isLargeArc
			};

			figure.Segments.Add(arcSegment);

			path.Data = new PathGeometry
			{
				Figures = new PathFigureCollection
				{
					figure
				}
			};

			List<Point> points = new List<Point>();

			GeometryHelper.FlattenArc(
				points,
				Point.Zero,
				arcSegment.Point,
				arcSegment.Size.Width,
				arcSegment.Size.Height,
				arcSegment.RotationAngle,
				arcSegment.IsLargeArc,
				arcSegment.SweepDirection == SweepDirection.CounterClockwise,
				1);

			Assert.AreNotEqual(0, points.Count);
		}

		[Fact]
		public void TestRoundLineGeometryConstruction()
		{
			var lineGeometry = new LineGeometry(new Point(0, 0), new Point(100, 100));

			Assert.IsNotNull(lineGeometry);
			Assert.Equal(0, lineGeometry.StartPoint.X);
			Assert.Equal(0, lineGeometry.StartPoint.Y);
			Assert.Equal(100, lineGeometry.EndPoint.X);
			Assert.Equal(100, lineGeometry.EndPoint.Y);
		}

		[Fact]
		public void TestEllipseGeometryConstruction()
		{
			var ellipseGeometry = new EllipseGeometry(new Point(50, 50), 10, 20);

			Assert.IsNotNull(ellipseGeometry);
			Assert.Equal(50, ellipseGeometry.Center.X);
			Assert.Equal(50, ellipseGeometry.Center.Y);
			Assert.Equal(10, ellipseGeometry.RadiusX);
			Assert.Equal(20, ellipseGeometry.RadiusY);
		}

		[Fact]
		public void TestRectangleGeometryConstruction()
		{
			var rectangleGeometry = new RectangleGeometry(new Rect(0, 0, 150, 150));

			Assert.IsNotNull(rectangleGeometry);
			Assert.Equal(150, rectangleGeometry.Rect.Height);
			Assert.Equal(150, rectangleGeometry.Rect.Width);
		}

		[Fact]
		public void TestRoundRectangleGeometryConstruction()
		{
			var roundRectangleGeometry = new RoundRectangleGeometry(new CornerRadius(12, 0, 0, 12), new Rect(0, 0, 150, 150));

			Assert.IsNotNull(roundRectangleGeometry);
			Assert.Equal(12, roundRectangleGeometry.CornerRadius.TopLeft);
			Assert.Equal(0, roundRectangleGeometry.CornerRadius.TopRight);
			Assert.Equal(0, roundRectangleGeometry.CornerRadius.BottomLeft);
			Assert.Equal(12, roundRectangleGeometry.CornerRadius.BottomRight);
			Assert.Equal(150, roundRectangleGeometry.Rect.Height);
			Assert.Equal(150, roundRectangleGeometry.Rect.Width);
		}
	}
}
