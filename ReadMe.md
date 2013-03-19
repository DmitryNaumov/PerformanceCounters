# Intro

Many times I was eager to add performance counter here and there... but it requires so much typing!!!

So I've decided to overcome it once and forever!

## Break & Enter

Declare

	[PerformanceCounterCategory("Sample Category", "Everything is so trivial", PerformanceCounterCategoryType.SingleInstance)]
	public interface SampleCounters : IPerformanceCounterSet
	{
		[PerformanceCounter("#Total Requests", "Total number of executed requests", PerformanceCounterType.NumberOfItems64)]
		IPerformanceCounter RequestsTotalCount { get; }

		[PerformanceCounter("#Executing Requests", "Number of currently executing requests", PerformanceCounterType.NumberOfItems32)]
		IPerformanceCounter RequestsExecutingCount { get; }

		[PerformanceCounter("Requests/sec", "Request rate", PerformanceCounterType.RateOfCountsPerSecond32)]
		IPerformanceCounter RequestRate { get; }

		[PerformanceCounter("Avg. Request Time", "Average request execution time", PerformanceCounterType.AverageTimer32)]
		IPerformanceCounter RequestAverageTime { get; }

		[PerformanceCounter("Avg. Request Time (base)", "Average request execution time", PerformanceCounterType.AverageBase)]
		IPerformanceCounter RequestAverageTimeBase { get; }
	}

Use

	var sampleCounters = PerformanceCounterFactory.GetCounters<SampleCounters>();
	sampleCounters.RequestsTotalCount.Increment();