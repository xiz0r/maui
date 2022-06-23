using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class CheckBoxUnitTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void TestConstructor()
		{
			var checkBox = new CheckBox();

			Assert.IsFalse(checkBox.IsChecked);
		}

		[Fact]
		public void TestOnEvent()
		{
			var checkBox = new CheckBox();

			var fired = false;
			checkBox.CheckedChanged += (sender, e) => fired = true;

			checkBox.IsChecked = true;

			Assert.IsTrue(fired);
		}

		[Fact]
		public void TestOnEventNotDoubleFired()
		{
			var checkBox = new CheckBox();

			bool fired = false;
			checkBox.IsChecked = true;

			checkBox.CheckedChanged += (sender, args) => fired = true;
			checkBox.IsChecked = true;

			Assert.IsFalse(fired);
		}
	}

}
