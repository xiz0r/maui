using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class DoubleCollectionTests
	{
		DoubleCollectionConverter _doubleCollectionConverter;

		
		public void SetUp()
		{
			_doubleCollectionConverter = new DoubleCollectionConverter();
		}

		[Fact]
		public void ConvertStringToDoubleCollectionTest()
		{
			DoubleCollection result = _doubleCollectionConverter.ConvertFromInvariantString("10,110 60,10 110,110") as DoubleCollection;

			Assert.IsNotNull(result);
			Assert.Equal(6, result.Count);
		}
	}
}