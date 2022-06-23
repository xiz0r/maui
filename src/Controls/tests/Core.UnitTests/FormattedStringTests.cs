using System;
using System.Collections.ObjectModel;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	
	public class FormattedStringTests : BaseTestFixtureXUnit
	{
		[Fact]
		public void NullSpansNotAllowed()
		{
			var fs = new FormattedString();
			Assert.That(() => fs.Spans.Add(null), Throws.InstanceOf<ArgumentNullException>());

			fs = new FormattedString();
			fs.Spans.Add(new Span());

			Assert.That(() =>
			{
				fs.Spans[0] = null;
			}, Throws.InstanceOf<ArgumentNullException>());
		}

		[Fact]
		public void SpanChangeTriggersSpansPropertyChange()
		{
			var span = new Span();
			var fs = new FormattedString();
			fs.Spans.Add(span);

			bool spansChanged = false;
			fs.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "Spans")
					spansChanged = true;
			};

			span.Text = "New text";

			Assert.That(spansChanged, Is.True);
		}

		[Fact]
		public void SpanChangesUnsubscribes()
		{
			var span = new Span();
			var fs = new FormattedString();
			fs.Spans.Add(span);
			fs.Spans.Remove(span);

			bool spansChanged = false;
			fs.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "Spans")
					spansChanged = true;
			};

			span.Text = "New text";

			Assert.That(spansChanged, Is.False);
		}

		[Fact]
		public void AddingSpanTriggersSpansPropertyChange()
		{
			var span = new Span();
			var fs = new FormattedString();

			bool spansChanged = false;
			fs.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "Spans")
					spansChanged = true;
			};

			fs.Spans.Add(span);

			Assert.That(spansChanged, Is.True);
		}

		[Fact]
		public void ImplicitStringConversion()
		{
			string original = "fubar";
			FormattedString fs = original;
			Assert.That(fs, Is.Not.Null);
			Assert.That(fs.Spans.Count, Is.EqualTo(1));
			Assert.That(fs.Spans[0], Is.Not.Null);
			Assert.That(fs.Spans[0].Text, Is.EqualTo(original));
		}

		[Fact]
		public void ImplicitStringConversionNull()
		{
			string original = null;
			FormattedString fs = original;
			Assert.That(fs, Is.Not.Null);
			Assert.That(fs.Spans.Count, Is.EqualTo(1));
			Assert.That(fs.Spans[0], Is.Not.Null);
			Assert.That(fs.Spans[0].Text, Is.EqualTo(original));
		}
	}
}