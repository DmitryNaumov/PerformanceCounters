namespace SampleApplication
{
	using System.Diagnostics;
	using PerformanceCounters;

	[PerformanceCounterCategory(".NET CLR Memory", "", PerformanceCounterCategoryType.MultiInstance)]
	public interface ClrMemoryCounters : IPerformanceCounterSet
	{
		[PerformanceCounter("% Time in GC", "", PerformanceCounterType.RawFraction)]
		IReadOnlyPerformanceCounter TimeInGC { get; }
	}
}