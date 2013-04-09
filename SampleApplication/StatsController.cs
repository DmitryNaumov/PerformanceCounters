namespace SampleApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using PerformanceCounters;

	public sealed class StatsController : ApiController
	{
		private readonly IPerformanceCounterSet[] _performanceCounters;

		public StatsController(IEnumerable<IPerformanceCounterSet> performanceCounters)
		{
			_performanceCounters = performanceCounters.ToArray();
		}

		public Category[] GetCategories()
		{
			return _performanceCounters.Select(pcs => new Category { Title = pcs.GetCategoryName() }).ToArray();
		}

		public Counter[] GetCounters(string categoryName)
		{
			var pcs = GetPerformanceCounterSet(categoryName);

			if (pcs == null)
			{
				return new Counter[0];
			}

			return pcs.GetCounterNames().Select(counterName => new Counter {Name = counterName}).ToArray();
		}

		public float[] GetCounterData(string categoryName)
		{
			var pcs = GetPerformanceCounterSet(categoryName);

			if (pcs == null)
			{
				return new float[0];
			}

			return pcs.GetCounterValues();
		}

		private IPerformanceCounterSet GetPerformanceCounterSet(string categoryName)
		{
			return _performanceCounters.FirstOrDefault(pcs => categoryName.Equals(pcs.GetCategoryName(), StringComparison.OrdinalIgnoreCase));
		}
	}

	public class Category
	{
		public string Title { get; set; }
	}

	public class Counter
	{
		public string Name { get; set; }
	}
}