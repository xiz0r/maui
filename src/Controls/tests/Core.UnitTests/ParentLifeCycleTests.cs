using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Xunit;
using NUnit.Framework.Constraints;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class ParentLifeCycleTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void ChangingAndChangedBothFireForParentOverride()
		{
			LifeCycleButton button = new LifeCycleButton();
			bool changing = false;
			bool changed = false;

			button.Parent = new Button();

			button.ParentChanging += (_, __) => changing = true;
			button.ParentChanged += (_, __) =>
			{
				if (!changing)
					Assert.Fail("Attached fired before changing");

				changed = true;
			};

			Assert.IsFalse(changing);
			Assert.IsFalse(changed);

			button.ParentOverride = new Button();

			Assert.IsTrue(changing);
			Assert.IsTrue(changed);
		}

		[Fact]
		public void ChangingAndChangedBothFireInitially()
		{
			LifeCycleButton button = new LifeCycleButton();
			bool changing = false;
			bool changed = false;

			button.ParentChanging += (_, __) => changing = true;
			button.ParentChanged += (_, __) =>
			{
				if (!changing)
					Assert.Fail("Attached fired before changing");

				changed = true;
			};

			Assert.Equal(0, button.changing);
			Assert.Equal(0, button.changed);
			Assert.IsFalse(changing);
			Assert.IsFalse(changed);

			button.Parent = new Button();

			Assert.IsTrue(changing);
			Assert.IsTrue(changed);
			Assert.Equal(1, button.changing);
			Assert.Equal(1, button.changed);
		}

		[Fact]
		public void ParentOverrideChangingArgsSendCorrectly()
		{
			LifeCycleButton button = new LifeCycleButton();

			Assert.IsNull(button.Parent);
			var firstParent = new Button();
			button.Parent = firstParent;

			Assert.Equal(button.LastParentChangingEventArgs.NewParent, firstParent);
			Assert.IsNull(button.LastParentChangingEventArgs.OldParent);

			var secondParent = new Button();
			button.ParentOverride = secondParent;
			Assert.Equal(button.LastParentChangingEventArgs.OldParent, firstParent);
			Assert.Equal(button.LastParentChangingEventArgs.NewParent, secondParent);

			button.ParentOverride = null;
			Assert.Equal(button.LastParentChangingEventArgs.OldParent, secondParent);
			Assert.Equal(button.LastParentChangingEventArgs.NewParent, firstParent);
		}

		[Fact]
		public void ChangingArgsAreSetCorrectly()
		{
			LifeCycleButton button = new LifeCycleButton();

			Assert.IsNull(button.Parent);
			var firstParent = new Button();
			button.Parent = firstParent;

			Assert.Equal(button.LastParentChangingEventArgs.NewParent, firstParent);
			Assert.IsNull(button.LastParentChangingEventArgs.OldParent);

			var secondParent = new Button();
			button.Parent = secondParent;
			Assert.Equal(button.LastParentChangingEventArgs.OldParent, firstParent);
			Assert.Equal(button.LastParentChangingEventArgs.NewParent, secondParent);

			button.Parent = null;
			Assert.Equal(button.LastParentChangingEventArgs.OldParent, secondParent);
			Assert.Equal(button.LastParentChangingEventArgs.NewParent, null);

			Assert.Equal(3, button.changing);
			Assert.Equal(3, button.changed);
		}

		public class LifeCycleButton : Button
		{
			public int changing = 0;
			public int changed = 0;
			public ParentChangingEventArgs LastParentChangingEventArgs { get; set; }

			protected override void OnParentChanging(ParentChangingEventArgs args)
			{
				LastParentChangingEventArgs = args;
				changing++;
				base.OnParentChanging(args);
			}

			protected override void OnParentChanged()
			{
				changed++;
				base.OnParentChanged();
			}
		}
	}
}
