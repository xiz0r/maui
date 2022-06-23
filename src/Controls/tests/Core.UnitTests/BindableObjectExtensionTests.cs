using System;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class BindableObjectExtensionTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void SetBindingNull()
		{
			Assert.That(() => BindableObjectExtensions.SetBinding(null, MockBindable.TextProperty, "Name"),
				Throws.InstanceOf<ArgumentNullException>());
			Assert.That(() => BindableObjectExtensions.SetBinding(new MockBindable(), null, "Name"),
				Throws.InstanceOf<ArgumentNullException>());
			Assert.That(() => BindableObjectExtensions.SetBinding(new MockBindable(), MockBindable.TextProperty, null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Fact]
		public void Issue2643()
		{
			Label labelTempoDiStampa = new Label();
			labelTempoDiStampa.BindingContext = new { Name = "1", Company = "Microsoft.Maui.Controls" };
			labelTempoDiStampa.SetBinding(Label.TextProperty, "Name", stringFormat: "Hi: {0}");

			Assert.That(labelTempoDiStampa.Text, Is.EqualTo("Hi: 1"));
		}

		class Bz27229ViewModel
		{
			public object Member { get; private set; }
			public Bz27229ViewModel()
			{
				Member = new Generic<Label>(new Label { Text = "foo" });
			}
		}

		class Generic<TResult>
		{
			public Generic(TResult result)
			{
				Result = result;
			}

			public TResult Result { get; set; }
		}

		[Fact]
		public void Bz27229()
		{
			var totalCheckTime = new TextCell { Text = "Total Check Time" };
			totalCheckTime.BindingContext = new Bz27229ViewModel();
			totalCheckTime.SetBinding(TextCell.DetailProperty, "Member.Result.Text");
			Assert.Equal("foo", totalCheckTime.Detail);
		}
	}
}
