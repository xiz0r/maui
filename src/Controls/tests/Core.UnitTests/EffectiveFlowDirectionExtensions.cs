using System.Linq;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class EffectiveFlowDirectionExtensions : BaseTestFixtureXUnit
	{
		[Fact]
		public void LeftToRightImplicit()
		{
			var target = FlowDirection.LeftToRight.ToEffectiveFlowDirection();

			Assert.IsTrue(target.IsLeftToRight());
			Assert.IsTrue(target.IsImplicit());

			Assert.IsFalse(target.IsRightToLeft());
			Assert.IsFalse(target.IsExplicit());
		}

		[Fact]
		public void LeftToRightExplicit()
		{
			var target = FlowDirection.LeftToRight.ToEffectiveFlowDirection(isExplicit: true);

			Assert.IsTrue(target.IsLeftToRight());
			Assert.IsTrue(target.IsExplicit());

			Assert.IsFalse(target.IsRightToLeft());
			Assert.IsFalse(target.IsImplicit());
		}

		[Fact]
		public void RightToLeftImplicit()
		{
			var target = FlowDirection.RightToLeft.ToEffectiveFlowDirection();

			Assert.IsTrue(target.IsRightToLeft());
			Assert.IsTrue(target.IsImplicit());

			Assert.IsFalse(target.IsLeftToRight());
			Assert.IsFalse(target.IsExplicit());
		}

		[Fact]
		public void RightToLeftExplicit()
		{
			var target = FlowDirection.RightToLeft.ToEffectiveFlowDirection(isExplicit: true);

			Assert.IsTrue(target.IsRightToLeft());
			Assert.IsTrue(target.IsExplicit());

			Assert.IsFalse(target.IsLeftToRight());
			Assert.IsFalse(target.IsImplicit());
		}
	}
}