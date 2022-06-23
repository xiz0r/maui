using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class LabelTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void TextAndAttributedTextMutuallyExclusive()
		{
			var label = new Label();
			Assert.IsNull(label.Text);
			Assert.IsNull(label.FormattedText);

			label.Text = "Foo";
			Assert.Equal("Foo", label.Text);
			Assert.IsNull(label.FormattedText);

			var fs = new FormattedString();
			label.FormattedText = fs;
			Assert.IsNull(label.Text);
			Assert.AreSame(fs, label.FormattedText);

			label.Text = "Foo";
			Assert.Equal("Foo", label.Text);
			Assert.IsNull(label.FormattedText);
		}

		[Fact]
		public void InvalidateMeasureWhenTextChanges()
		{
			var label = new Label();

			bool fired;
			label.MeasureInvalidated += (sender, args) =>
			{
				fired = true;
			};

			fired = false;
			label.Text = "Foo";
			Assert.IsTrue(fired);

			fired = false;
			label.TextTransform = TextTransform.Lowercase;
			Assert.IsTrue(fired);

			fired = false;
			label.TextTransform = TextTransform.Uppercase;
			Assert.IsTrue(fired);

			fired = false;
			label.TextTransform = TextTransform.None;
			Assert.IsTrue(fired);

			var fs = new FormattedString();

			fired = false;
			label.FormattedText = fs;
			Assert.IsTrue(fired);

			fired = false;
			fs.Spans.Add(new Span { Text = "bar" });
			Assert.IsTrue(fired);
		}

		[Fact]
		public void AssignedToFontSizeDouble()
		{
			var label = new Label();

			label.FontSize = 10.7;
			Assert.Equal(label.FontSize, 10.7);
		}

		[Fact]
		public void LabelResizesWhenFontChanges()
		{
			MockPlatformSizeService.Current.GetPlatformSizeFunc = (ve, w, h) =>
			{
				var l = (Label)ve;
				return new SizeRequest(new Size(l.FontSize, l.FontSize));
			};

			var label = new Label { IsPlatformEnabled = true };

			Assert.Equal(label.FontSize, label.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Width);

			bool fired = false;

			label.MeasureInvalidated += (sender, args) =>
			{
				Assert.Equal(25, label.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Width);
				fired = true;
			};


			label.FontSize = 25;

			Assert.True(fired);
		}

		[Fact]
		public void FontSizeConverterTests()
		{
			var converter = new FontSizeConverter();
			Assert.Equal(12, converter.ConvertFromInvariantString("12"));
			Assert.Equal(10.7, converter.ConvertFromInvariantString("10.7"));
		}

		[Fact]
		public void FontSizeCanBeSetFromStyle()
		{
			var label = new Label();

			Assert.Equal(label.GetDefaultFontSize(), label.FontSize);

			label.SetValue(Label.FontSizeProperty, 1.0, true);
			Assert.Equal(1.0, label.FontSize);
		}

		[Fact]
		public void ManuallySetFontSizeNotOverridenByStyle()
		{
			var label = new Label();
			Assume.That(label.FontSize, Is.EqualTo(label.GetDefaultFontSize()));

			label.SetValue(Label.FontSizeProperty, 2.0, false);
			Assert.Equal(2.0, label.FontSize);

			label.SetValue(Label.FontSizeProperty, 1.0, true);
			Assert.Equal(2.0, label.FontSize);
		}

		[Fact]
		public void ManuallySetFontSizeNotOverridenByFontSetInStyle()
		{
			var label = new Label();
			Assume.That(label.FontSize, Is.EqualTo(label.GetDefaultFontSize()));

			label.SetValue(Label.FontSizeProperty, 2.0);
			Assert.Equal(2.0, label.FontSize);
		}

		[Fact]
		public void EntryCellXAlignBindingMatchesHorizontalTextAlignmentBinding()
		{
			var vm = new ViewModel();
			vm.HorizontalAlignment = TextAlignment.Center;

			var labelHorizontalTextAlignment = new Label() { BindingContext = vm };
			labelHorizontalTextAlignment.SetBinding(Label.HorizontalTextAlignmentProperty, new Binding("HorizontalAlignment"));

			Assert.Equal(TextAlignment.Center, labelHorizontalTextAlignment.HorizontalTextAlignment);

			vm.HorizontalAlignment = TextAlignment.End;

			Assert.Equal(TextAlignment.End, labelHorizontalTextAlignment.HorizontalTextAlignment);
		}

		[Fact]
		public void EntryCellYAlignBindingMatchesVerticalTextAlignmentBinding()
		{
			var vm = new ViewModel();
			vm.VerticalAlignment = TextAlignment.Center;

			var labelVerticalTextAlignment = new Label() { BindingContext = vm };
			labelVerticalTextAlignment.SetBinding(Label.VerticalTextAlignmentProperty, new Binding("VerticalAlignment"));

			Assert.Equal(TextAlignment.Center, labelVerticalTextAlignment.VerticalTextAlignment);

			vm.VerticalAlignment = TextAlignment.End;

			Assert.Equal(TextAlignment.End, labelVerticalTextAlignment.VerticalTextAlignment);
		}

		sealed class ViewModel : INotifyPropertyChanged
		{
			TextAlignment horizontalAlignment;
			TextAlignment verticalAlignment;

			public TextAlignment HorizontalAlignment
			{
				get { return horizontalAlignment; }
				set
				{
					horizontalAlignment = value;
					OnPropertyChanged();
				}
			}

			public TextAlignment VerticalAlignment
			{
				get { return verticalAlignment; }
				set
				{
					verticalAlignment = value;
					OnPropertyChanged();
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}

