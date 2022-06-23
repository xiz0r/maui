using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class ProgressBarTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void TestClamp()
		{
			ProgressBar bar = new ProgressBar();

			bar.Progress = 2;
			Assert.Equal(1, bar.Progress);

			bar.Progress = -1;
			Assert.Equal(0, bar.Progress);
		}

		[Fact]
		public void TestProgressTo()
		{
			var bar = AnimationReadyHandler.Prepare(new ProgressBar());

			bar.ProgressTo(0.8, 250, Easing.Linear);

			Assert.That(bar.Progress, Is.EqualTo(0.8).Within(0.001));
		}
	}
}