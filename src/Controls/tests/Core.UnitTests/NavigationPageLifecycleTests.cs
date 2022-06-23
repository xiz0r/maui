using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class NavigationPageLifecycleTests : BaseTestFixtureXUnit
	{
		[TestCase(false)]
		[TestCase(true)]
		public async Task AppearingFiresForInitialPage(bool useMaui)
		{
			ContentPage contentPage = new ContentPage();
			ContentPage resultPage = null;

			contentPage.Appearing += (sender, _)
				=> resultPage = (ContentPage)sender;

			NavigationPage nav = new TestNavigationPage(useMaui, contentPage);

			Assert.IsNull(resultPage);
			_ = new Window(nav);
			Assert.Equal(resultPage, contentPage);
		}


		[TestCase(false)]
		[TestCase(true)]
		public async Task PushLifeCycle(bool useMaui)
		{
			ContentPage initialPage = new ContentPage();
			ContentPage pushedPage = new ContentPage();

			ContentPage initialPageDisappear = null;
			ContentPage pageAppearing = null;

			initialPage.Disappearing += (sender, _)
				=> initialPageDisappear = (ContentPage)sender;

			pushedPage.Appearing += (sender, _)
				=> pageAppearing = (ContentPage)sender;

			NavigationPage nav = new TestNavigationPage(useMaui, initialPage);
			_ = new Window(nav);
			nav.SendAppearing();

			await nav.PushAsync(pushedPage);

			Assert.Equal(initialPageDisappear, initialPage);
			Assert.Equal(pageAppearing, pushedPage);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task PopLifeCycle(bool useMaui)
		{
			ContentPage initialPage = new ContentPage();
			ContentPage pushedPage = new ContentPage();

			ContentPage rootPageFiresAppearingAfterPop = null;
			ContentPage pageDisappeared = null;

			NavigationPage nav = new TestNavigationPage(useMaui, initialPage);
			_ = new Window(nav);
			nav.SendAppearing();

			initialPage.Appearing += (sender, _)
				=> rootPageFiresAppearingAfterPop = (ContentPage)sender;

			pushedPage.Disappearing += (sender, _)
				=> pageDisappeared = (ContentPage)sender;

			await nav.PushAsync(pushedPage);
			Assert.IsNull(rootPageFiresAppearingAfterPop);
			Assert.IsNull(pageDisappeared);

			await nav.PopAsync();

			Assert.Equal(initialPage, rootPageFiresAppearingAfterPop);
			Assert.Equal(pushedPage, pageDisappeared);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task RemoveLastPage(bool useMaui)
		{
			ContentPage initialPage = new ContentPage();
			ContentPage pushedPage = new ContentPage();

			ContentPage initialPageAppearing = null;
			ContentPage pageDisappeared = null;

			NavigationPage nav = new TestNavigationPage(useMaui, initialPage);
			_ = new Window(nav);
			nav.SendAppearing();

			initialPage.Appearing += (sender, _)
				=> initialPageAppearing = (ContentPage)sender;

			pushedPage.Disappearing += (sender, _)
				=> pageDisappeared = (ContentPage)sender;

			await nav.PushAsync(pushedPage);
			Assert.IsNull(initialPageAppearing);
			Assert.IsNull(pageDisappeared);

			nav.Navigation.RemovePage(pushedPage);

			Assert.Equal(initialPageAppearing, initialPage);
			Assert.Equal(pageDisappeared, pushedPage);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task RemoveInnerPage(bool useMaui)
		{
			ContentPage initialPage = new ContentPage();
			ContentPage pushedPage = new ContentPage();

			ContentPage initialPageAppearing = null;
			ContentPage pageDisappeared = null;

			NavigationPage nav = new TestNavigationPage(useMaui, initialPage);
			_ = new Window(nav);
			nav.SendAppearing();

			var pageToRemove = new ContentPage();
			await nav.PushAsync(pageToRemove);
			await nav.PushAsync(pushedPage);


			initialPage.Appearing += (__, _)
				=> Assert.Fail("Appearing Fired Incorrectly");

			pushedPage.Disappearing += (__, _)
				=> Assert.Fail("Appearing Fired Incorrectly");

			nav.Navigation.RemovePage(pageToRemove);
		}
	}
}
