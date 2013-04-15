namespace SampleApplication
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
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
			return _performanceCounters.Select(pcs => new Category { Title = pcs.CategoryName }).ToArray();
		}

		public Counter[] GetCounters(string categoryName)
		{
			var pcs = GetPerformanceCounterSet(categoryName);

			if (pcs == null)
			{
				return new Counter[0];
			}

			return pcs.Counters.Where(counter => !IsBaseCounter(counter)).Select(counter => new Counter {Name = counter.CounterName}).ToArray();
		}

		public float[] GetCounterData(string categoryName)
		{
			var pcs = GetPerformanceCounterSet(categoryName);

			if (pcs == null)
			{
				return new float[0];
			}

			return pcs.Counters.Select(pc => pc.NextValue()).ToArray();
		}

		private IPerformanceCounterSet GetPerformanceCounterSet(string categoryName)
		{
			return _performanceCounters.FirstOrDefault(pcs => categoryName.Equals(pcs.CategoryName, StringComparison.OrdinalIgnoreCase));
		}

		private bool IsBaseCounter(IPerformanceCounter counter)
		{
			switch (counter.CounterType)
			{
				case PerformanceCounterType.AverageBase:
				case PerformanceCounterType.RawBase:
				case PerformanceCounterType.SampleBase:
				case PerformanceCounterType.CounterMultiBase:
					return true;
			}

			return false;
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