using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class OpenGLViewUnitTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void Display()
		{
			var view = new OpenGLView();
			bool displayed = false;

			view.DisplayRequested += (s, o) => displayed = true;

			view.Display();
			Assert.True(displayed);
		}
	}
}
