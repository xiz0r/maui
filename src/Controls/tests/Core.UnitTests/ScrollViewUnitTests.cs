using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Graphics;
using NSubstitute;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	using StackLayout = Microsoft.Maui.Controls.Compatibility.StackLayout;

	
	public class ScrollViewUnitTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void TestConstructor()
		{
			ScrollView scrollView = new ScrollView();

			Assert.Null(scrollView.Content);

			View view = new View();
			scrollView = new ScrollView { Content = view };

			Assert.Equal(view, scrollView.Content);
		}

		[Fact]
		[InlineData(ScrollOrientation.Horizontal)]
		[InlineData(ScrollOrientation.Both)]
		public void GetsCorrectSizeRequestWithWrappingContent(ScrollOrientation orientation)
		{
			MockPlatformSizeService.Current.UseRealisticLabelMeasure = true;

			var scrollView = new ScrollView
			{
				IsPlatformEnabled = true,
				Orientation = orientation,
			};

			var hLayout = new StackLayout
			{
				IsPlatformEnabled = true,
				Orientation = StackOrientation.Horizontal,
				Children = {
					new Label {Text = "THIS IS A REALLY LONG STRING", IsPlatformEnabled = true},
					new Label {Text = "THIS IS A REALLY LONG STRING", IsPlatformEnabled = true},
					new Label {Text = "THIS IS A REALLY LONG STRING", IsPlatformEnabled = true},
					new Label {Text = "THIS IS A REALLY LONG STRING", IsPlatformEnabled = true},
					new Label {Text = "THIS IS A REALLY LONG STRING", IsPlatformEnabled = true},
				}
			};

			scrollView.Content = hLayout;

			var r = scrollView.Measure(100, 100);

			Assert.Equal(10, r.Request.Height);
		}

		[Fact]
		public void TestChildChanged()
		{
			ScrollView scrollView = new ScrollView();

			bool changed = false;
			scrollView.PropertyChanged += (sender, e) =>
			{
				switch (e.PropertyName)
				{
					case "Content":
						changed = true;
						break;
				}
			};
			View view = new View();
			scrollView.Content = view;

			Assert.True(changed);
		}

		[Fact]
		public void TestChildDoubleSet()
		{
			var scrollView = new ScrollView();

			bool changed = false;
			scrollView.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Content")
					changed = true;
			};

			var child = new View();
			scrollView.Content = child;

			Assert.True(changed);
			Assert.Equal(child, scrollView.Content);
			Assert.Equal(child.Parent, scrollView);

			changed = false;

			scrollView.Content = child;

			Assert.False(changed);

			scrollView.Content = null;

			Assert.True(changed);
			Assert.Null(scrollView.Content);
			Assert.Null(child.Parent);
		}

		[Fact]
		public void TestOrientation()
		{
			var scrollView = new ScrollView();

			Assert.Equal(ScrollOrientation.Vertical, scrollView.Orientation);

			bool signaled = false;
			scrollView.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Orientation")
					signaled = true;
			};

			scrollView.Orientation = ScrollOrientation.Horizontal;

			Assert.Equal(ScrollOrientation.Horizontal, scrollView.Orientation);
			Assert.True(signaled);

			scrollView.Orientation = ScrollOrientation.Both;
			Assert.Equal(ScrollOrientation.Both, scrollView.Orientation);
			Assert.True(signaled);

			scrollView.Orientation = ScrollOrientation.Neither;
			Assert.Equal(ScrollOrientation.Neither, scrollView.Orientation);
			Assert.True(signaled);
		}

		[Fact]
		public void TestOrientationDoubleSet()
		{
			var scrollView = new ScrollView();

			bool signaled = false;
			scrollView.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Orientation")
					signaled = true;
			};

			scrollView.Orientation = scrollView.Orientation;

			Assert.False(signaled);
		}


		[Fact]
		public void TestScrollTo()
		{
			var scrollView = new ScrollView();

			var item = new View { };
			scrollView.Content = new StackLayout { Children = { item } };

			bool requested = false;
			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				requested = true;
				Assert.Equal(args.ScrollY, 100);
				Assert.Equal(args.ScrollX, 0);
				Assert.Null(args.Item);
				Assert.Equal(args.ShouldAnimate, true);
			};

			scrollView.ScrollToAsync(0, 100, true);
			Assert.That(requested, Is.True);
		}

		[Fact]
		public void TestScrollWasNotFiredOnNeither()
		{
			var scrollView = new ScrollView
			{
				Orientation = ScrollOrientation.Neither
			};

			var item = new View { };
			scrollView.Content = new StackLayout { Children = { item } };

			bool requested = false;
			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				requested = true;
			};

			scrollView.ScrollToAsync(0, 100, true);
			Assert.False(requested);
		}

		[Fact]
		public void TestScrollToNotAnimated()
		{
			var scrollView = new ScrollView();

			var item = new View { };
			scrollView.Content = new StackLayout { Children = { item } };

			bool requested = false;
			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				requested = true;
				Assert.Equal(args.ScrollY, 100);
				Assert.Equal(args.ScrollX, 0);
				Assert.Null(args.Item);
				Assert.Equal(args.ShouldAnimate, false);
			};

			scrollView.ScrollToAsync(0, 100, false);
			Assert.That(requested, Is.True);
		}

		[Fact]
		public void TestScrollToElement()
		{
			var scrollView = new ScrollView();

			var item = new Label { Text = "Test" };
			scrollView.Content = new StackLayout { Children = { item } };

			bool requested = false;
			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				requested = true;

				Assert.That(args.Element, Is.SameAs(item));
				Assert.Equal(args.Position, ScrollToPosition.Center);
				Assert.Equal(args.ShouldAnimate, true);
			};

			scrollView.ScrollToAsync(item, ScrollToPosition.Center, true);
			Assert.That(requested, Is.True);
		}

		[Fact]
		public void TestScrollToElementNotAnimated()
		{
			var scrollView = new ScrollView();

			var item = new Label { Text = "Test" };
			scrollView.Content = new StackLayout { Children = { item } };

			bool requested = false;
			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				requested = true;

				Assert.That(args.Element, Is.SameAs(item));
				Assert.Equal(args.Position, ScrollToPosition.Center);
				Assert.Equal(args.ShouldAnimate, false);
			};

			scrollView.ScrollToAsync(item, ScrollToPosition.Center, false);
			Assert.That(requested, Is.True);
		}

		[Fact]
		public void TestScrollToInvalid()
		{
			var scrollView = new ScrollView();

			Assert.That(() => scrollView.ScrollToAsync(new VisualElement(), ScrollToPosition.Center, true), Throws.ArgumentException);
			Assert.That(() => scrollView.ScrollToAsync(null, (ScrollToPosition)500, true), Throws.ArgumentException);
		}

		[Fact]
		public void SetScrollPosition()
		{
			var scroll = new ScrollView();
			IScrollViewController controller = scroll;
			controller.SetScrolledPosition(100, 100);

			Assert.Equal(scroll.ScrollX, 100);
			Assert.Equal(scroll.ScrollY, 100);
		}

		[Fact]
		public void TestBackToBackBiDirectionalScroll()
		{
			var scrollView = new ScrollView
			{
				Orientation = ScrollOrientation.Both,
				Content = new Grid
				{
					WidthRequest = 1000,
					HeightRequest = 1000
				}
			};

			var y100Count = 0;

			((IScrollViewController)scrollView).ScrollToRequested += (sender, args) =>
			{
				if (args.ScrollY == 100)
				{
					++y100Count;
				}
			};

			scrollView.ScrollToAsync(100, 100, true);
			Assert.Equal(y100Count, 1);

			scrollView.ScrollToAsync(0, 100, true);
			Assert.Equal(y100Count, 2);
		}

		void AssertInvalidated(IViewHandler handler)
		{
			handler.Received().Invoke(Arg.Is(nameof(IView.InvalidateMeasure)), Arg.Any<object>());
			handler.ClearReceivedCalls();
		}
	}
}
