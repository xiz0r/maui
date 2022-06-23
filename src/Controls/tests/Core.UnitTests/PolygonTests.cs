using Microsoft.Maui.Controls.Shapes;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class PolygonTests : BaseTestFixtureXUnit
	{
		PointCollectionConverter _pointCollectionConverter;

		
		public override void Setup()
		{
			

			_pointCollectionConverter = new PointCollectionConverter();
		}

		[Fact]
		public void CreatePolygonFromStringPointCollectionTest()
		{
			PointCollection points = _pointCollectionConverter.ConvertFromInvariantString("0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48") as PointCollection;

			Polygon polygon = new Polygon
			{
				Points = points
			};

			Assert.IsNotNull(points);
			Assert.IsNotNull(polygon);
			Assert.Equal(10, points.Count);
		}
	}
}