﻿using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class PageLifeCycleTests : BaseTestFixtureXUnit
	{
		[TestCase(false)]
		[TestCase(true)]
		public void NavigationPageInitialPage(bool useMaui)
		{
			var lcPage = new LCPage();
			NavigationPage navigationPage = new TestNavigationPage(useMaui, lcPage);
			navigationPage.InitialNativeNavigationStackLoaded();
			Assert.IsNull(lcPage.NavigatingFromArgs);
			Assert.IsNull(lcPage.NavigatedFromArgs);
			Assert.NotNull(lcPage.NavigatedToArgs);
			Assert.IsNull(lcPage.NavigatedToArgs.PreviousPage);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task NavigationPagePushPage(bool useMaui)
		{
			var previousPage = new LCPage();
			var lcPage = new LCPage();
			NavigationPage navigationPage = new TestNavigationPage(useMaui, previousPage);
			await navigationPage.PushAsync(lcPage);

			Assert.IsNotNull(previousPage.NavigatingFromArgs);
			Assert.Equal(previousPage, lcPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(lcPage, previousPage.NavigatedFromArgs.DestinationPage);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task NavigationPagePopPage(bool useMaui)
		{
			var firstPage = new LCPage();
			var poppedPage = new LCPage();

			NavigationPage navigationPage = new TestNavigationPage(useMaui, firstPage);
			await navigationPage.PushAsync(poppedPage);
			await navigationPage.PopAsync();

			Assert.IsNotNull(poppedPage.NavigatingFromArgs);
			Assert.Equal(poppedPage, firstPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(firstPage, poppedPage.NavigatedFromArgs.DestinationPage);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task NavigationPagePopToRoot(bool useMaui)
		{
			var firstPage = new LCPage();
			var poppedPage = new LCPage();

			NavigationPage navigationPage = new TestNavigationPage(useMaui, firstPage);
			await navigationPage.PushAsync(new ContentPage());
			await navigationPage.PushAsync(new ContentPage());
			await navigationPage.PushAsync(poppedPage);
			await navigationPage.PopToRootAsync();

			Assert.IsNotNull(poppedPage.NavigatingFromArgs);
			Assert.Equal(poppedPage, firstPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(firstPage, poppedPage.NavigatedFromArgs.DestinationPage);
		}

		[Fact]
		public async Task TabbedPageBasicSelectionChanged()
		{
			var firstPage = new LCPage() { Title = "First Page" };
			var secondPage = new LCPage() { Title = "Second Page" };
			var tabbedPage = new TabbedPage() { Children = { firstPage, secondPage } };

			tabbedPage.CurrentPage = secondPage;
			Assert.IsNotNull(firstPage.NavigatingFromArgs);
			Assert.Equal(firstPage, secondPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(secondPage, firstPage.NavigatedFromArgs.DestinationPage);
		}

		[Fact]
		public void TabbedPageInitialPage()
		{
			var firstPage = new LCPage() { Title = "First Page" };
			var secondPage = new LCPage() { Title = "Second Page" };
			var tabbedPage = new TabbedPage() { Children = { firstPage, secondPage } };
			Assert.IsNull(firstPage.NavigatingFromArgs);
			Assert.IsNull(firstPage.NavigatedFromArgs);
			Assert.NotNull(firstPage.NavigatedToArgs);
			Assert.IsNull(firstPage.NavigatedToArgs.PreviousPage);
		}

		[Fact]
		public async Task FlyoutPageFlyoutChanged()
		{
			var firstPage = new LCPage() { Title = "First Page" };
			var secondPage = new LCPage() { Title = "Second Page" };
			var flyoutPage = new FlyoutPage() { Flyout = firstPage };
			flyoutPage.Flyout = secondPage;

			Assert.IsNotNull(firstPage.NavigatingFromArgs);
			Assert.Equal(firstPage, secondPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(secondPage, firstPage.NavigatedFromArgs.DestinationPage);
		}

		[Fact]
		public async Task FlyoutPageDetailChanged()
		{
			var firstPage = new LCPage() { Title = "First Page" };
			var secondPage = new LCPage() { Title = "Second Page" };
			var flyoutPage = new FlyoutPage() { Detail = firstPage };
			flyoutPage.Detail = secondPage;

			Assert.IsNotNull(firstPage.NavigatingFromArgs);
			Assert.Equal(firstPage, secondPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(secondPage, firstPage.NavigatedFromArgs.DestinationPage);
		}

		[Fact]
		public async Task PushModalPage()
		{
			var previousPage = new LCPage();
			var lcPage = new LCPage();
			var window = new Window(previousPage);

			await window.Navigation.PushModalAsync(lcPage);

			Assert.IsNotNull(previousPage.NavigatingFromArgs);
			Assert.Equal(previousPage, lcPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(lcPage, previousPage.NavigatedFromArgs.DestinationPage);

			Assert.Equal(1, previousPage.DisappearingCount);
			Assert.Equal(1, lcPage.AppearingCount);
		}

		[Fact]
		public async Task PopModalPage()
		{
			var firstPage = new LCPage();
			var poppedPage = new LCPage();

			var window = new Window(firstPage);
			await window.Navigation.PushModalAsync(poppedPage);
			await window.Navigation.PopModalAsync();

			Assert.IsNotNull(poppedPage.NavigatingFromArgs);
			Assert.Equal(poppedPage, firstPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(firstPage, poppedPage.NavigatedFromArgs.DestinationPage);

			Assert.Equal(1, poppedPage.AppearingCount);
			Assert.Equal(1, poppedPage.DisappearingCount);
			Assert.Equal(2, firstPage.AppearingCount);
		}

		[Fact]
		public async Task PopToAModalPage()
		{
			var firstPage = new LCPage();
			var firstModalPage = new LCPage();
			var secondModalPage = new LCPage();

			var window = new Window(firstPage);
			await window.Navigation.PushModalAsync(firstModalPage);
			await window.Navigation.PushModalAsync(secondModalPage);

			firstModalPage.ClearNavigationArgs();
			secondModalPage.ClearNavigationArgs();

			await window.Navigation.PopModalAsync();

			Assert.IsNotNull(secondModalPage.NavigatingFromArgs);
			Assert.Equal(secondModalPage, firstModalPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(firstModalPage, secondModalPage.NavigatedFromArgs.DestinationPage);

			Assert.Equal(1, secondModalPage.DisappearingCount);
			Assert.Equal(1, secondModalPage.AppearingCount);

			Assert.Equal(1, firstModalPage.DisappearingCount);
			Assert.Equal(2, firstModalPage.AppearingCount);
		}

		[Fact]
		public async Task PushSecondModalPage()
		{
			var firstPage = new LCPage();
			var firstModalPage = new LCPage();
			var secondModalPage = new LCPage();

			var window = new Window(firstPage);
			await window.Navigation.PushModalAsync(firstModalPage);

			firstModalPage.ClearNavigationArgs();
			secondModalPage.ClearNavigationArgs();

			await window.Navigation.PushModalAsync(secondModalPage);

			Assert.IsNotNull(firstModalPage.NavigatingFromArgs);
			Assert.Equal(firstModalPage, secondModalPage.NavigatedToArgs.PreviousPage);
			Assert.Equal(secondModalPage, firstModalPage.NavigatedFromArgs.DestinationPage);

			Assert.Equal(0, secondModalPage.DisappearingCount);
			Assert.Equal(1, secondModalPage.AppearingCount);

			Assert.Equal(1, firstModalPage.DisappearingCount);
			Assert.Equal(1, firstModalPage.AppearingCount);
		}

		class LCPage : ContentPage
		{
			public NavigatedFromEventArgs NavigatedFromArgs { get; private set; }
			public NavigatingFromEventArgs NavigatingFromArgs { get; private set; }
			public NavigatedToEventArgs NavigatedToArgs { get; private set; }
			public int AppearingCount { get; private set; }
			public int DisappearingCount { get; private set; }

			public void ClearNavigationArgs()
			{
				NavigatedFromArgs = null;
				NavigatingFromArgs = null;
				NavigatedToArgs = null;
			}

			protected override void OnAppearing()
			{
				base.OnAppearing();
				AppearingCount++;
			}

			protected override void OnDisappearing()
			{
				base.OnDisappearing();
				DisappearingCount++;
			}

			protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
			{
				base.OnNavigatedFrom(args);
				NavigatedFromArgs = args;
			}

			protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
			{
				base.OnNavigatingFrom(args);
				NavigatingFromArgs = args;
			}

			protected override void OnNavigatedTo(NavigatedToEventArgs args)
			{
				base.OnNavigatedTo(args);
				NavigatedToArgs = args;
			}
		}
	}
}
