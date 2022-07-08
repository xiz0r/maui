using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls.Internals;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class ResourceDictionaryTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void Add()
		{
			var rd = new ResourceDictionary();
			rd.Add("foo", "bar");
			Assert.Equal("bar", rd["foo"]);
		}

		[Fact]
		public void AddKVP()
		{
			var rd = new ResourceDictionary();
			((ICollection<KeyValuePair<string, object>>)rd).Add(new KeyValuePair<string, object>("foo", "bar"));
			Assert.Equal("bar", rd["foo"]);
		}

		[Fact]
		public void ResourceDictionaryTriggersValueChangedOnAdd()
		{
			var rd = new ResourceDictionary();
			((IResourceDictionary)rd).ValuesChanged += (sender, e) =>
			{
				Assert.Equal(1, e.Values.Count());
				var kvp = e.Values.First();
				Assert.Equal("foo", kvp.Key);
				Assert.Equal("FOO", kvp.Value);
				Assert.Pass();
			};
			rd.Add("foo", "FOO");
			Assert.Fail();
		}

		[Fact]
		public void ResourceDictionaryTriggersValueChangedOnChange()
		{
			var rd = new ResourceDictionary();
			rd.Add("foo", "FOO");
			((IResourceDictionary)rd).ValuesChanged += (sender, e) =>
			{
				Assert.Equal(1, e.Values.Count());
				var kvp = e.Values.First();
				Assert.Equal("foo", kvp.Key);
				Assert.Equal("BAR", kvp.Value);
				Assert.Pass();
			};
			rd["foo"] = "BAR";
			Assert.Fail();
		}

		[Fact]
		public void ResourceDictionaryCtor()
		{
			var rd = new ResourceDictionary();
			Assert.Equal(0, rd.Count());
		}

		[Fact]
		public void ElementMergesParentRDWithCurrent()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAR" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "foo", "FOO" },
				}
			};

			elt.Parent = parent;

			object value;
			Assert.True(elt.TryGetResource("foo", out value));
			Assert.Equal("FOO", value);
			Assert.True(elt.TryGetResource("bar", out value));
			Assert.Equal("BAR", value);
		}

		[Fact]
		public void CurrentOverridesParentValues()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAZ" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "foo", "FOO" },
					{ "bar","BAR" },
				}
			};

			elt.Parent = parent;

			object value;
			Assert.True(elt.TryGetResource("foo", out value));
			Assert.Equal("FOO", value);
			Assert.True(elt.TryGetResource("bar", out value));
			Assert.Equal("BAZ", value);
		}

		[Fact]
		public void AddingToParentTriggersValuesChanged()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAR" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "foo", "FOO" },
				}
			};

			elt.Parent = parent;

			((IElementDefinition)elt).AddResourcesChangedListener((sender, e) =>
			{
				Assert.Equal(1, e.Values.Count());
				var kvp = e.Values.First();
				Assert.Equal("baz", kvp.Key);
				Assert.Equal("BAZ", kvp.Value);
				Assert.Pass();
			});

			parent.Resources["baz"] = "BAZ";
			Assert.Fail();
		}

		[Fact]
		public void ResourcesChangedNotRaisedIfKeyExistsInCurrent()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAR" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "foo", "FOO" },
				}
			};

			elt.Parent = parent;

			((IElementDefinition)elt).AddResourcesChangedListener((sender, e) => Assert.Fail());
			parent.Resources["bar"] = "BAZ";
			Assert.Pass();
		}

		[Fact]
		public void SettingParentTriggersValuesChanged()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAR" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{"foo", "FOO"},
					{"baz", "BAZ"},
					{"bar", "NEWBAR"}
				}
			};

			((IElementDefinition)elt).AddResourcesChangedListener((sender, e) =>
			{
				Assert.Equal(2, e.Values.Count());
				Assert.Equal("FOO", e.Values.First(kvp => kvp.Key == "foo").Value);
				Assert.Equal("BAZ", e.Values.First(kvp => kvp.Key == "baz").Value);
				Assert.Pass();
			});
			elt.Parent = parent;
			Assert.Fail();
		}

		[Fact]
		public void SettingResourcesTriggersResourcesChanged()
		{
			var elt = new VisualElement();

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{"bar", "BAR"}
				}
			};

			elt.Parent = parent;

			((IElementDefinition)elt).AddResourcesChangedListener((sender, e) =>
			{
				Assert.Equal(3, e.Values.Count());
				Assert.Pass();
			});
			elt.Resources = new ResourceDictionary {
				{"foo", "FOO"},
				{"baz", "BAZ"},
				{"bar", "NEWBAR"}
			};
			Assert.Fail();
		}

		[Fact]
		public void DontThrowOnReparenting()
		{
			var elt = new View { Resources = new ResourceDictionary() };
			var parent = new StackLayout();

			parent.Children.Add(elt);
			Assert.DoesNotThrow(() => parent.Children.Remove(elt));
		}

		[Fact]
		public void MultiLevelMerge()
		{
			var elt = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "bar","BAR" },
				}
			};

			var parent = new VisualElement
			{
				Resources = new ResourceDictionary {
					{ "foo", "FOO" },
				},
				Parent = new VisualElement
				{
					Resources = new ResourceDictionary {
						{"baz", "BAZ"}
					}
				}
			};

			((IElementDefinition)elt).AddResourcesChangedListener((sender, e) =>
			{
				Assert.Equal(2, e.Values.Count());
				Assert.Pass();
			});

			elt.Parent = parent;
			Assert.Fail();
		}

		[Fact]
		public void ShowKeyInExceptionIfNotFound()
		{
			var rd = new ResourceDictionary();
			rd.Add("foo", "bar");
			var ex = Assert.Throws<KeyNotFoundException>(() => { var foo = rd["test_invalid_key"]; });
			Assert.That(ex.Message, Does.Contain("test_invalid_key"));
		}

		class MyRD : ResourceDictionary
		{
			public MyRD()
			{
				CreationCount = CreationCount + 1;
				Add("foo", "Foo");
				Add("bar", "Bar");
			}

			public static int CreationCount { get; set; }
		}

		[Fact]
		public void ThrowOnDuplicateKey()
		{
			var rd0 = new ResourceDictionary();
			rd0.Add("foo", "Foo");
			try
			{
				rd0.Add("foo", "Bar");
			}
			catch (ArgumentException ae)
			{
				Assert.Equal("A resource with the key 'foo' is already present in the ResourceDictionary.", ae.Message);
				Assert.Pass();
			}
			Assert.Fail();
		}

		[Fact]
		public void MergedDictionaryResourcesAreFound()
		{
			var rd0 = new ResourceDictionary();
			rd0.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar" } });

			object value;
			Assert.True(rd0.TryGetValue("foo", out value));
			Assert.Equal("bar", value);
		}

		[Fact]
		public void MergedDictionaryResourcesAreFoundLastDictionaryTakesPriority()
		{
			var rd0 = new ResourceDictionary();
			rd0.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar" } });
			rd0.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar1" } });
			rd0.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar2" } });

			object value;
			Assert.True(rd0.TryGetValue("foo", out value));
			Assert.Equal("bar2", value);
		}

		[Fact]
		public void CountDoesNotIncludeMergedDictionaries()
		{
			var rd = new ResourceDictionary {
				{"baz", "Baz"},
				{"qux", "Qux"},
			};
			rd.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar" } });

			Assert.Equal(rd.Count, 2);
		}

		[Fact]
		public void ClearMergedDictionaries()
		{
			var rd = new ResourceDictionary {
				{"baz", "Baz"},
				{"qux", "Qux"},
			};
			rd.MergedDictionaries.Add(new ResourceDictionary() { { "foo", "bar" } });

			Assert.Equal(rd.Count, 2);

			rd.MergedDictionaries.Clear();

			Assert.Equal(rd.MergedDictionaries.Count, 0);
		}

		[Fact]
		public void AddingMergedRDTriggersValueChanged()
		{
			var rd = new ResourceDictionary();
			var label = new Label
			{
				Resources = rd
			};
			label.SetDynamicResource(Label.TextProperty, "foo");
			Assert.Equal(label.Text, Label.TextProperty.DefaultValue);

			rd.MergedDictionaries.Add(new ResourceDictionary { { "foo", "Foo" } });
			Assert.Equal(label.Text, "Foo");
		}

		[Fact]
		//this is to keep the alignment with resources removed from RD
		public void RemovingMergedRDDoesntTriggersValueChanged()
		{
			var rd = new ResourceDictionary
			{
				MergedDictionaries = {
					new ResourceDictionary {
						{ "foo", "Foo" }
					}
				}
			};
			var label = new Label
			{
				Resources = rd,
			};

			label.SetDynamicResource(Label.TextProperty, "foo");
			Assert.Equal(label.Text, "Foo");

			rd.MergedDictionaries.Clear();
			Assert.Equal(label.Text, "Foo");
		}

		[Fact]
		public void AddingResourceInMergedRDTriggersValueChanged()
		{
			var rd0 = new ResourceDictionary();
			var rd = new ResourceDictionary
			{
				MergedDictionaries = {
					rd0
				}
			};

			var label = new Label
			{
				Resources = rd,
			};
			label.SetDynamicResource(Label.TextProperty, "foo");
			Assert.Equal(label.Text, Label.TextProperty.DefaultValue);

			rd0.Add("foo", "Foo");
			Assert.Equal(label.Text, "Foo");
		}
	}
}