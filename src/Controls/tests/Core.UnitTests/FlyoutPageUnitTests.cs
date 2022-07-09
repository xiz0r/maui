using System;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class FlyoutPageUnitTests : BaseTestFixtureXUnit
	{
		MockDeviceDisplay mockDeviceDisplay;
		MockDeviceInfo mockDeviceInfo;

		
		public FlyoutPageUnitTests()
		{
			
			DeviceDisplay.SetCurrent(mockDeviceDisplay = new MockDeviceDisplay());
			DeviceInfo.SetCurrent(mockDeviceInfo = new MockDeviceInfo());
		}

		[Fact]
		public void TestConstructor()
		{
			FlyoutPage page = new FlyoutPage();

			Assert.Null(page.Flyout);
			Assert.Null(page.Detail);
		}

		[Fact]
		public void TestFlyoutSetter()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label(), Title = "Foo" };
			page.Flyout = child;

			Assert.Equal(child, page.Flyout);
		}

		[Fact]
		public void TestFlyoutSetNull()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label(), Title = "Foo" };
			page.Flyout = child;

			Assert.Throws<ArgumentNullException>(() => { page.Flyout = null; });
		}

		[Fact]
		public void TestFlyoutChanged()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label(), Title = "Foo" };

			bool changed = false;
			page.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == "Flyout")
					changed = true;
			};

			page.Flyout = child;

			Assert.True(changed);
		}

		[Fact]
		public void TestDetailSetter()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label() };
			page.Detail = child;

			Assert.Equal(child, page.Detail);
		}

		[Fact]
		public void TestDetailSetNull()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label() };
			page.Detail = child;

			Assert.Throws<ArgumentNullException>(() => { page.Detail = null; });
		}

		[Fact]
		public void TestDetailChanged()
		{
			FlyoutPage page = new FlyoutPage();
			var child = new ContentPage { Content = new Label() };

			bool changed = false;
			page.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == "Detail")
					changed = true;
			};

			page.Detail = child;

			Assert.True(changed);
		}

		[Theory, InlineData(null), InlineData("")]
		public void ThrowsWhenFlyoutSetWithoutValidTitle(string title)
		{
			var page = new FlyoutPage();
			Assert.Throws<InvalidOperationException>(() => page.Flyout = new ContentPage { Title = title });
		}

		[Fact]
		public void TestThrowsWhenPackedWithoutSetting()
		{
			FlyoutPage page = new FlyoutPage();
			Assert.Throws<InvalidOperationException>(() => new TabbedPage { Children = { page } });
		}

		[Fact]
		public void TestDoesNotThrowWhenPackedWithSetting()
		{
			FlyoutPage page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), Title = "Foo" },
				Detail = new ContentPage { Content = new View() }
			};
			_ = new TabbedPage { Children = { page } };
		}

		[Fact]
		public void TestFlyoutVisible()
		{
			var page = new FlyoutPage();

			Assert.False(page.IsPresented);

			bool signaled = false;
			page.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == FlyoutPage.IsPresentedProperty.PropertyName)
					signaled = true;
			};

			page.IsPresented = true;

			Assert.True(page.IsPresented);
			Assert.True(signaled);
		}

		[Fact]
		public void TestFlyoutVisibleDoubleSet()
		{
			var page = new FlyoutPage();

			bool signaled = false;
			page.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == FlyoutPage.IsPresentedProperty.PropertyName)
					signaled = true;
			};

			page.IsPresented = page.IsPresented;

			Assert.False(signaled);
		}

		[Fact]
		public void TestSetFlyoutBounds()
		{
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), Title = "Foo" },
				Detail = new ContentPage { Content = new View() }
			};

			((IFlyoutPageController)page).FlyoutBounds = new Rect(0, 0, 100, 100);
			Assert.Equal(new Rect(0, 0, 100, 100), page.Flyout.Bounds);
			Assert.Equal(new Rect(0, 0, 100, 100), ((IFlyoutPageController)page).FlyoutBounds);
		}

		[Fact]
		public void TestSetDetailBounds()
		{
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), Title = "Foo" },
				Detail = new ContentPage { Content = new View() }
			};

			((IFlyoutPageController)page).DetailBounds = new Rect(0, 0, 100, 100);
			Assert.Equal(new Rect(0, 0, 100, 100), page.Detail.Bounds);
			Assert.Equal(new Rect(0, 0, 100, 100), ((IFlyoutPageController)page).DetailBounds);
		}

		[Fact]
		public void TestLayoutChildren()
		{
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), IsPlatformEnabled = true, Title = "Foo" },
				Detail = new ContentPage { Content = new View(), IsPlatformEnabled = true },
				IsPlatformEnabled = true,
			};

			((IFlyoutPageController)page).FlyoutBounds = new Rect(0, 0, 100, 200);
			((IFlyoutPageController)page).DetailBounds = new Rect(0, 0, 100, 100);

			page.Flyout.Layout(new Rect(0, 0, 1, 1));
			page.Detail.Layout(new Rect(0, 0, 1, 1));

			page.Layout(new Rect(0, 0, 200, 200));

			Assert.Equal(new Rect(0, 0, 100, 200), page.Flyout.Bounds);
			Assert.Equal(new Rect(0, 0, 100, 100), page.Detail.Bounds);
		}

		[Fact]
		public void ThorwsInLayoutChildrenWithNullDetail()
		{
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), IsPlatformEnabled = true, Title = "Foo" },
				IsPlatformEnabled = true,
			};

			Assert.Throws<InvalidOperationException>(() => page.Layout(new Rect(0, 0, 200, 200)));
		}

		[Fact]
		public void ThorwsInLayoutChildrenWithNullFlyout()
		{
			var page = new FlyoutPage
			{
				Detail = new ContentPage { Content = new View(), IsPlatformEnabled = true },
				IsPlatformEnabled = true,
			};

			Assert.Throws<InvalidOperationException>(() => page.Layout(new Rect(0, 0, 200, 200)));
		}

		[Fact]
		public void ThorwsInSetDetailBoundsWithNullDetail()
		{
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), Title = "Foo" },
				IsPlatformEnabled = true,
			};

			Assert.Throws<InvalidOperationException>(() => ((IFlyoutPageController)page).DetailBounds = new Rect(0, 0, 200, 200));
		}

		[Fact]
		public void ThrowsInSetFlyoutBoundsWithNullFlyout()
		{
			var page = new FlyoutPage
			{
				Detail = new ContentPage { Content = new View() },
				IsPlatformEnabled = true,
			};

			Assert.Throws<InvalidOperationException>(() => ((IFlyoutPageController)page).FlyoutBounds = new Rect(0, 0, 200, 200));
		}

		[Fact]
		public void ThrowsInSetIsPresentOnSplitModeOnTablet()
		{
			mockDeviceInfo.Idiom = DeviceIdiom.Tablet;
			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), IsPlatformEnabled = true, Title = "Foo" },
				Detail = new ContentPage { Content = new View(), IsPlatformEnabled = true },
				IsPlatformEnabled = true,
				FlyoutLayoutBehavior = FlyoutLayoutBehavior.Split
			};

			Assert.Throws<InvalidOperationException>(() => page.IsPresented = false);
		}

		[Fact]
		public void ThrowsInSetIsPresentOnSplitPortraitModeOnTablet()
		{
			mockDeviceInfo.Idiom = DeviceIdiom.Tablet;
			mockDeviceDisplay.SetMainDisplayOrientation(DisplayOrientation.Portrait);

			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), IsPlatformEnabled = true, Title = "Foo" },
				Detail = new ContentPage { Content = new View(), IsPlatformEnabled = true },
				FlyoutLayoutBehavior = FlyoutLayoutBehavior.SplitOnPortrait
			};

			Assert.Throws<InvalidOperationException>(() => page.IsPresented = false);
		}

		[Fact]
		public void TestSetIsPresentedOnPopoverMode()
		{
			mockDeviceDisplay.SetMainDisplayOrientation(DisplayOrientation.Landscape);

			var page = new FlyoutPage
			{
				Flyout = new ContentPage { Content = new View(), IsPlatformEnabled = true, Title = "Foo" },
				Detail = new ContentPage { Content = new View(), IsPlatformEnabled = true },
				IsPlatformEnabled = true,
				FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover
			};
			page.IsPresented = true;

			Assert.True(page.IsPresented);
		}

		[Fact]
		public void SendsBackEventToPresentedFlyoutFirst()
		{
			var detail = new BackButtonPage() { Handle = true };
			var Flyout = new BackButtonPage() { Title = "Flyout" };
			var mdp = new FlyoutPage()
			{
				Detail = detail,
				Flyout = Flyout,
				IsPresented = true,
				IsPlatformEnabled = true,
			};

			((IFlyoutPageController)mdp).BackButtonPressed += (sender, args) =>
			{
				args.Handled = mdp.IsPresented;
				mdp.IsPresented = false;
			};

			var detailEmitted = false;
			var FlyoutEmitted = false;

			detail.BackPressed += (sender, args) => detailEmitted = true;
			Flyout.BackPressed += (sender, args) => FlyoutEmitted = true;

			var result = mdp.SendBackButtonPressed();

			Assert.True(FlyoutEmitted);
			Assert.False(detailEmitted);
			Assert.True(result);
		}

		[Fact]
		public void EmitsCorrectlyWhenPresentedOnBackPressed()
		{
			var detail = new BackButtonPage();
			var Flyout = new BackButtonPage { Title = "Flyout" };
			var mdp = new FlyoutPage
			{
				Detail = detail,
				Flyout = Flyout,
				IsPresented = true,
				IsPlatformEnabled = true,
			};

			((IFlyoutPageController)mdp).BackButtonPressed += (sender, args) =>
			{
				args.Handled = mdp.IsPresented;
				mdp.IsPresented = false;
			};

			var detailEmitted = false;
			var FlyoutEmitted = false;

			detail.BackPressed += (sender, args) => detailEmitted = true;
			Flyout.BackPressed += (sender, args) => FlyoutEmitted = true;

			var result = mdp.SendBackButtonPressed();

			Assert.True(FlyoutEmitted);
			Assert.False(detailEmitted);
			Assert.True(result);
		}

		[Fact]
		public void ThrowsExceptionWhenAddingAlreadyParentedDetail()
		{
			var detail = new ContentPage { };

			// give detail a parent
			var nav = new NavigationPage(detail);

			var mdp = new FlyoutPage();
			Assert.Throws<InvalidOperationException>(() => mdp.Detail = detail);
		}

		[Fact]
		public void ThrowsExceptionWhenAddingAlreadyParentedFlyout()
		{
			var Flyout = new ContentPage { Title = "Foo" };

			// give Flyout a parent
			var nav = new NavigationPage(Flyout);

			var mdp = new FlyoutPage();
			Assert.Throws<InvalidOperationException>(() => mdp.Flyout = Flyout);
		}
	}

}
