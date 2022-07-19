using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	[TestFixture]
	public class HandlerLifeCycleTests : BaseTestFixture
	{
		[Test]
		public void SettingHandlerToNullDisconnectsHandlerFromVirtualView()
		{
			var mauiApp1 = MauiApp.CreateBuilder()
				.UseMauiApp<ApplicationStub>()
				.ConfigureMauiHandlers(handlers => handlers.AddHandler<LifeCycleButton, HandlerStub>())
				.Build();

			LifeCycleButton button = new LifeCycleButton();

			MauiContext mauiContext1 = new MauiContext(mauiApp1.Services);
			var handler1 = button.ToHandler(mauiContext1);
			button.Handler = null;
			Assert.AreNotEqual(button, handler1.VirtualView);
		}

		[Test]
		public void SettingNewHandlerDisconnectsOldHandler()
		{
			var mauiApp1 = MauiApp.CreateBuilder()
				.UseMauiApp<ApplicationStub>()
				.ConfigureMauiHandlers(handlers => handlers.AddHandler<LifeCycleButton, HandlerStub>())
				.Build();

			LifeCycleButton button = new LifeCycleButton();

			MauiContext mauiContext1 = new MauiContext(mauiApp1.Services);
			var handler1 = button.ToHandler(mauiContext1);

			var mauiApp2 = MauiApp.CreateBuilder()
				.UseMauiApp<ApplicationStub>()
				.ConfigureMauiHandlers(handlers => handlers.AddHandler<LifeCycleButton, HandlerStub>())
				.Build();

			MauiContext mauiContext2 = new MauiContext(mauiApp2.Services);

			List<HandlerChangingEventArgs> changingArgs = new List<HandlerChangingEventArgs>();
			button.HandlerChanging += (s, a) =>
			{
				Assert.AreEqual(handler1, a.OldHandler);
				Assert.NotNull(a.NewHandler);

				changingArgs.Add(a);
			};

			var handler2 = button.ToHandler(mauiContext2);

			Assert.AreNotEqual(button, handler1.VirtualView);
			Assert.AreEqual(button, handler2.VirtualView);
			Assert.AreEqual(1, changingArgs.Count);
		}

		[Test]
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

			Assert.AreEqual(0, button.changing);
			Assert.AreEqual(0, button.changed);
			Assert.IsFalse(changing);
			Assert.IsFalse(changed);

			button.Handler = new HandlerStub();

			Assert.IsTrue(changing);
			Assert.IsTrue(changed);
			Assert.AreEqual(1, button.changing);
			Assert.AreEqual(1, button.changed);
		}

		[Test]
		public void ChangingArgsAreSetCorrectly()
		{
			LifeCycleButton button = new LifeCycleButton();

			Assert.IsNull(button.Handler);
			var firstHandler = new HandlerStub();
			button.Handler = firstHandler;

			Assert.AreEqual(button.LastHandlerChangingEventArgs.NewHandler, firstHandler);
			Assert.IsNull(button.LastHandlerChangingEventArgs.OldHandler);

			var secondHandler = new HandlerStub();
			button.Handler = secondHandler;
			Assert.AreEqual(button.LastHandlerChangingEventArgs.OldHandler, firstHandler);
			Assert.AreEqual(button.LastHandlerChangingEventArgs.NewHandler, secondHandler);

			button.Handler = null;
			Assert.AreEqual(button.LastHandlerChangingEventArgs.OldHandler, secondHandler);
			Assert.AreEqual(button.LastHandlerChangingEventArgs.NewHandler, null);

			Assert.AreEqual(3, button.changing);
			Assert.AreEqual(3, button.changed);
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
