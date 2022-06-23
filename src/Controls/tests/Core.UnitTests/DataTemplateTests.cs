using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class DataTemplateTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void CtorInvalid()
		{
			Assert.Throws<ArgumentNullException>(() => new DataTemplate((Func<object>)null),
				"Allowed null creator delegate");

			Assert.Throws<ArgumentNullException>(() => new DataTemplate((Type)null),
				"Allowed null type");
		}

		[Fact]
		public void CreateContent()
		{
			var template = new DataTemplate(() => new MockBindable());
			object obj = template.CreateContent();

			Assert.NotNull(obj);
			Assert.That(obj, Is.InstanceOf<MockBindable>());
		}

		[Fact]
		public void CreateContentType()
		{
			var template = new DataTemplate(typeof(MockBindable));
			object obj = template.CreateContent();

			Assert.NotNull(obj);
			Assert.That(obj, Is.InstanceOf<MockBindable>());
		}

		[Fact]
		public void CreateContentValues()
		{
			var template = new DataTemplate(typeof(MockBindable))
			{
				Values = { { MockBindable.TextProperty, "value" } }
			};

			MockBindable bindable = (MockBindable)template.CreateContent();
			Assert.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo("value"));
		}

		[Fact]
		public void CreateContentBindings()
		{
			var template = new DataTemplate(() => new MockBindable())
			{
				Bindings = { { MockBindable.TextProperty, new Binding(".") } }
			};

			MockBindable bindable = (MockBindable)template.CreateContent();
			bindable.BindingContext = "text";
			Assert.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo("text"));
		}

		[Fact]
		public void SetBindingInvalid()
		{
			var template = new DataTemplate(typeof(MockBindable));
			Assert.That(() => template.SetBinding(null, new Binding(".")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That(() => template.SetBinding(MockBindable.TextProperty, null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Fact]
		public void SetBindingOverridesValue()
		{
			var template = new DataTemplate(typeof(MockBindable));
			template.SetValue(MockBindable.TextProperty, "value");
			template.SetBinding(MockBindable.TextProperty, new Binding("."));

			MockBindable bindable = (MockBindable)template.CreateContent();
			Assume.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo(bindable.BindingContext));

			bindable.BindingContext = "binding";
			Assert.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo("binding"));
		}

		[Fact]
		public void SetValueOverridesBinding()
		{
			var template = new DataTemplate(typeof(MockBindable));
			template.SetBinding(MockBindable.TextProperty, new Binding("."));
			template.SetValue(MockBindable.TextProperty, "value");

			MockBindable bindable = (MockBindable)template.CreateContent();
			Assert.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo("value"));
			bindable.BindingContext = "binding";
			Assert.That(bindable.GetValue(MockBindable.TextProperty), Is.EqualTo("value"));
		}

		[Fact]
		public void SetValueInvalid()
		{
			var template = new DataTemplate(typeof(MockBindable));
			Assert.That(() => template.SetValue(null, "string"), Throws.InstanceOf<ArgumentNullException>());
		}

		[Fact]
		public void SetValueAndBinding()
		{
			var template = new DataTemplate(typeof(TextCell))
			{
				Bindings = {
					{TextCell.TextProperty, new Binding ("Text")}
				},
				Values = {
					{TextCell.TextProperty, "Text"}
				}
			};
			Assert.That(() => template.CreateContent(), Throws.InstanceOf<InvalidOperationException>());
		}

		[Fact]
		public void HotReloadTransitionDoesNotCrash()
		{
			// Hot Reload may need to create a template while the content portion isn't ready yet
			// We need to make sure that a call to CreateContent during that time doesn't crash
			var template = new DataTemplate();
			Assert.DoesNotThrow(() => template.CreateContent());
		}
	}
}
