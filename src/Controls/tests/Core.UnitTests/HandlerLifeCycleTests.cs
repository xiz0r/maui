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
	
	public class HandlerLifeCycleTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void ChangingAndChangedBothFireInitially()
		{
			LifeCycleButton button = new LifeCycleButton();
			bool changing = false;
			bool changed = false;

			button.HandlerChanging += (_, __) => changing = true;
			button.HandlerChanged += (_, __) =>
			{
				if (!changing)
					Assert.Fail("Attached fired before changing");

				changed = true;
			};

			Assert.Equal(0, button.changing);
			Assert.Equal(0, button.changed);
			Assert.IsFalse(changing);
			Assert.IsFalse(changed);

			button.Handler = new HandlerStub();

			Assert.IsTrue(changing);
			Assert.IsTrue(changed);
			Assert.Equal(1, button.changing);
			Assert.Equal(1, button.changed);
		}

		[Fact]
		public void ChangingArgsAreSetCorrectly()
		{
			LifeCycleButton button = new LifeCycleButton();

			Assert.IsNull(button.Handler);
			var firstHandler = new HandlerStub();
			button.Handler = firstHandler;

			Assert.Equal(button.LastHandlerChangingEventArgs.NewHandler, firstHandler);
			Assert.IsNull(button.LastHandlerChangingEventArgs.OldHandler);

			var secondHandler = new HandlerStub();
			button.Handler = secondHandler;
			Assert.Equal(button.LastHandlerChangingEventArgs.OldHandler, firstHandler);
			Assert.Equal(button.LastHandlerChangingEventArgs.NewHandler, secondHandler);

			button.Handler = null;
			Assert.Equal(button.LastHandlerChangingEventArgs.OldHandler, secondHandler);
			Assert.Equal(button.LastHandlerChangingEventArgs.NewHandler, null);

			Assert.Equal(3, button.changing);
			Assert.Equal(3, button.changed);
		}

		public class LifeCycleButton : Button
		{
			public int changing = 0;
			public int changed = 0;
			public HandlerChangingEventArgs LastHandlerChangingEventArgs { get; set; }

			protected override void OnHandlerChanging(HandlerChangingEventArgs args)
			{
				LastHandlerChangingEventArgs = args;
				changing++;
				base.OnHandlerChanging(args);
			}

			protected override void OnHandlerChanged()
			{
				changed++;
				base.OnHandlerChanged();

				if (changed != changing)
					Assert.Fail("Attaching/Attached fire mismatch");
			}
		}
	}
}
