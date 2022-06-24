using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	static class TestDataHelpers 
	{
		static IEnumerable<IEnumerable<T>> CartesianProduct<T>(
			this IEnumerable<IEnumerable<T>> sequences)
		{
			IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
			return sequences.Aggregate(
			  emptyProduct,
			  (accumulator, sequence) =>
				from accseq in accumulator
				from item in sequence
				select accseq.Concat(new[] { item }));
		}

		public static IEnumerable<object[]> Combinations<T>(IEnumerable<IEnumerable<T>> inputs) 
		{
			foreach (var set in CartesianProduct(inputs))
			{
				yield return set.Cast<object>().ToArray();
			}
		}

		public static IEnumerable<object[]> Combinations<T>(IEnumerable<T> inputs) 
		{
			var all = new List<IEnumerable<T>>(inputs.Count());
			foreach (var i in inputs)
			{
				all.Add(new List<T>(inputs));
			}

			return Combinations(all);
		}

		public static IEnumerable<object[]> TrueFalseData()
		{
			return TestDataHelpers.Combinations(new List<bool>() { true, false });
		}



		// [Theory, InlineData(true), InlineData(false)]
	}
}
