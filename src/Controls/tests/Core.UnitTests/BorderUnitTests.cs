#nullable enable
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class BorderUnitTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void HasNoVisualChildrenWhenNoContentIsSet()
		{
			var border = new Border();

			Assert.IsEmpty(((IVisualTreeElement)border).GetVisualChildren());

			border.Content = null;

			Assert.IsEmpty(((IVisualTreeElement)border).GetVisualChildren());
		}

		[Fact]
		public void HasVisualChildrenWhenContentIsSet()
		{
			var border = new Border();

			var label = new Label();
			border.Content = label;

			var visualTreeChildren = ((IVisualTreeElement)border).GetVisualChildren();
			Assert.IsNotEmpty(visualTreeChildren);
			Assert.AreSame(visualTreeChildren[0], label);
		}

		[Fact]
		public void ChildrenHaveParentsWhenContentIsSet()
		{
			var border = new Border();

			var label = new Label();
			border.Content = label;

			Assert.AreSame(border, label.Parent);

			border.Content = null;
			Assert.Null(label.Parent);
		}
	}
}