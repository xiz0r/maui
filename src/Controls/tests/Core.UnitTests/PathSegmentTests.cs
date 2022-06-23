using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class PathSegmentTests : BaseTestFixtureXUnit
	{
		PointCollectionConverter _pointCollectionConverter;

		
		public override void Setup()
		{
			

			_pointCollectionConverter = new PointCollectionConverter();
		}

		[Fact]
		public void TestArcSegmentConstructor()
		{
			var arcSegment1 = new ArcSegment();
			Assert.IsNotNull(arcSegment1);

			var arcSegment2 = new ArcSegment(new Point(0, 0), new Size(100, 100), 90, SweepDirection.Clockwise, false);
			Assert.IsNotNull(arcSegment2);
			Assert.Equal(90, arcSegment2.RotationAngle);
			Assert.Equal(100, arcSegment2.Size.Height);
			Assert.Equal(100, arcSegment2.Size.Width);
		}

		[Fact]
		public void TestBezierSegmentConstructor()
		{
			var bezierSegment1 = new BezierSegment();
			Assert.IsNotNull(bezierSegment1);

			var bezierSegment2 = new BezierSegment(new Point(0, 0), new Point(50, 50), new Point(100, 100));
			Assert.IsNotNull(bezierSegment2);
			Assert.Equal(0, bezierSegment2.Point1.X);
			Assert.Equal(0, bezierSegment2.Point1.Y);
			Assert.Equal(50, bezierSegment2.Point2.X);
			Assert.Equal(50, bezierSegment2.Point2.Y);
			Assert.Equal(100, bezierSegment2.Point3.X);
			Assert.Equal(100, bezierSegment2.Point3.Y);
		}

		[Fact]
		public void TestLineSegmentConstructor()
		{
			var lineSegment1 = new LineSegment();
			Assert.IsNotNull(lineSegment1);

			var lineSegment2 = new LineSegment(new Point(25, 50));
			Assert.IsNotNull(lineSegment2);
			Assert.Equal(25, lineSegment2.Point.X);
			Assert.Equal(50, lineSegment2.Point.Y);
		}

		[TestCase("", 0)]
		[TestCase("0 48", 1)]
		[TestCase("0 48, 0 144", 2)]
		[TestCase("0 48, 0 144, 96 150", 3)]
		[TestCase("0 48, 0 144, 96 150, 100 0", 4)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0", 5)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96", 6)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96", 7)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192", 8)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200", 9)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48", 10)]
		public void TestPolyBezierSegmentConstructor(string points, int count)
		{
			var pointCollection = (PointCollection)_pointCollectionConverter.ConvertFromInvariantString(points);

			var polyBezierSegment = new PolyBezierSegment(pointCollection);
			Assert.IsNotNull(polyBezierSegment);
			Assert.Equal(count, polyBezierSegment.Points.Count);
		}

		[TestCase("", 0)]
		[TestCase("0 48", 1)]
		[TestCase("0 48, 0 144", 2)]
		[TestCase("0 48, 0 144, 96 150", 3)]
		[TestCase("0 48, 0 144, 96 150, 100 0", 4)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0", 5)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96", 6)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96", 7)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192", 8)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200", 9)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48", 10)]
		public void TestPolyLineSegmentConstructor(string points, int count)
		{
			var pointCollection = (PointCollection)_pointCollectionConverter.ConvertFromInvariantString(points);
			var polyLineSegment = new PolyLineSegment(pointCollection);
			Assert.IsNotNull(polyLineSegment);
			Assert.Equal(count, polyLineSegment.Points.Count);
		}

		[TestCase("", 0)]
		[TestCase("0 48", 1)]
		[TestCase("0 48, 0 144", 2)]
		[TestCase("0 48, 0 144, 96 150", 3)]
		[TestCase("0 48, 0 144, 96 150, 100 0", 4)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0", 5)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96", 6)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96", 7)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192", 8)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200", 9)]
		[TestCase("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48", 10)]
		public void TestPolyQuadraticBezierSegmentConstructor(string points, int count)
		{
			var pointCollection = (PointCollection)_pointCollectionConverter.ConvertFromInvariantString(points);
			var polyQuadraticBezierSegment = new PolyQuadraticBezierSegment(pointCollection);
			Assert.IsNotNull(polyQuadraticBezierSegment);
			Assert.Equal(count, polyQuadraticBezierSegment.Points.Count);
		}

		[Fact]
		public void TestQuadraticBezierSegmentConstructor()
		{
			var quadraticBezierSegment1 = new QuadraticBezierSegment();
			Assert.IsNotNull(quadraticBezierSegment1);

			var quadraticBezierSegment2 = new QuadraticBezierSegment(new Point(0, 0), new Point(100, 100));
			Assert.IsNotNull(quadraticBezierSegment2);
			Assert.Equal(0, quadraticBezierSegment2.Point1.X);
			Assert.Equal(0, quadraticBezierSegment2.Point1.Y);
			Assert.Equal(100, quadraticBezierSegment2.Point2.X);
			Assert.Equal(100, quadraticBezierSegment2.Point2.Y);
		}
	}
}