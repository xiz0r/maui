using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		int count;

		public MainPage()
		{
			InitializeComponent();


			SetLabelBackgroundColor = new Command<string>(
				execute: (string arg) =>
				{
					LabelBackgroundColor = Color.Parse(arg);
					OnPropertyChanged(nameof(LabelBackgroundColor));
				});

			BindingContext = this;

			//wv.HandlerChanged += OnWebViewHandlerChanged;
		}

//		private async void OnWebViewHandlerChanged(object sender, EventArgs e)
//		{
//			var platformView = wv.Handler?.PlatformView;
//			if (platformView != null)
//			{
//#if WINDOWS
//				var windowsWebView = (Microsoft.UI.Xaml.Controls.WebView2)platformView;
//				await windowsWebView.EnsureCoreWebView2Async();
//				windowsWebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
//#else
//				await Task.Delay(0);
//#endif
//			}
//		}

		public ICommand SetLabelBackgroundColor { get; init; }

		public ImageSource HappyImageSource { get; } = new FontImageSource { Glyph = ":)", FontFamily = "Arial", Color = Colors.Blue, };
		public ImageSource MediumImageSource { get; } = new FontImageSource { Glyph = ":|", FontFamily = "Arial", Color = Colors.Red, };
		public ImageSource NotHappyImageSource { get; } = new FontImageSource { Glyph = ":(", FontFamily = "Arial", Color = Colors.Green, };
		public ImageSource CurrentIconImageSource { get; set; } = new FontImageSource { Glyph = "?", FontFamily = "Arial", Color = Colors.Purple, };

		public Color LabelBackgroundColor { get; set; } = Colors.Aqua;

		private void Button_Clicked(object sender, EventArgs e)
		{
			count++;
			OnPropertyChanged(nameof(CounterValue));
		}

		private void OnSuperIncrementMenuItemClicked(object sender, EventArgs e)
		{
			var menuItem = (MenuItem)sender;
			var incrementAmount = int.Parse((string)menuItem.CommandParameter);
			count += incrementAmount;
			OnPropertyChanged(nameof(CounterValue));
		}

		public string CounterValue => count.ToString("N0");

		private async void OnHappinessMenuItemClicked(object sender, EventArgs e)
		{
			await DisplayAlert(
				title: "Happiness",
				message: $"The menu is: {_happiness}",
				cancel: "OK");
		}

		string _happiness = "unknown";

		private void OnChooseHappyMenuItemClicked(object sender, EventArgs e)
		{
			_happiness = "happy";
			CurrentIconImageSource = HappyImageSource;
			OnPropertyChanged(nameof(CurrentIconImageSource));
		}

		private void OnChooseMediumMenuItemClicked(object sender, EventArgs e)
		{
			_happiness = "medium";
			CurrentIconImageSource = MediumImageSource;
			OnPropertyChanged(nameof(CurrentIconImageSource));
		}

		private void OnChooseNotHappyMenuItemClicked(object sender, EventArgs e)
		{
			_happiness = "not happy";
			CurrentIconImageSource = NotHappyImageSource;
			OnPropertyChanged(nameof(CurrentIconImageSource));
		}

		private async void OnWebViewMenuItemClicked(object sender, EventArgs e)
		{
			await DisplayAlert(
				title: "WebView",
				message: $"The web view was clicked!",
				cancel: "OK");
		}

		private async void OnEntryMenuItemClicked(object sender, EventArgs e)
		{
			await DisplayAlert(
				title: "Entry",
				message: $"The entry was clicked!",
				cancel: "OK");
		}

		private void MakeLabelRed(object sender, EventArgs e) => ColorLabel.BackgroundColor = Colors.Red;
		private void MakeLabelBlue(object sender, EventArgs e) => ColorLabel.BackgroundColor = Colors.LightBlue;
		private void MakeLabelHotPink(object sender, EventArgs e) => ColorLabel.BackgroundColor = Colors.HotPink;
		private void MakeLabelPaleGoldenrod(object sender, EventArgs e) => ColorLabel.BackgroundColor = Colors.PaleGoldenrod;
	}
}
