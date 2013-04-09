namespace SampleApplication
{
	using System;
	using System.Web.Http;
	using PerformanceCounters;

	public sealed class StatsController : ApiController
	{
		private readonly ClrMemoryCounters _clrMemoryCounters;

		public StatsController(ClrMemoryCounters clrMemoryCounters)
		{
			_clrMemoryCounters = clrMemoryCounters;
		}

		public Category[] GetCategories()
		{
			return new[]
			{
				new Category()
				{
					Title = "System",
				},
				new Category()
				{
					Title = "Application"
				},
			};
		}

		public Counter[] GetCounters(string categoryName)
		{
			if (categoryName != "System")
			{
				return new Counter[0];
			}

			return new []
			{
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.BytesInAllHeaps)
				},
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.Gen0Collections)
				},
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.Gen1Collections)
				},
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.Gen2Collections)
				},
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.AllocatedBytesPerSecond)
				},
				new Counter
				{
					Name = _clrMemoryCounters.GetCounterName(x => x.TimeInGC)
				},
			};
		}

		public float[] GetCounterData()
		{
			return new[]
			{
				_clrMemoryCounters.BytesInAllHeaps.NextValue(),
				_clrMemoryCounters.Gen0Collections.NextValue(),
				_clrMemoryCounters.Gen1Collections.NextValue(),
				_clrMemoryCounters.Gen2Collections.NextValue(),
				_clrMemoryCounters.AllocatedBytesPerSecond.NextValue(),
				_clrMemoryCounters.TimeInGC.NextValue(),
			};
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