namespace SampleApplication
{
	using System.Diagnostics;
	using NeedfulThings.PerformanceCounters;

	[PerformanceCounterCategory(".NET CLR Memory", PerformanceCounterCategoryType.MultiInstance)]
	public interface ClrMemoryCounters : IPerformanceCounterSet
	{
		[PerformanceCounter("# Bytes in all Heaps")]
		IReadOnlyPerformanceCounter BytesInAllHeaps { get; }

		[PerformanceCounter("# Gen 0 Collections")]
		IReadOnlyPerformanceCounter Gen0Collections { get; }

		[PerformanceCounter("# Gen 1 Collections")]
		IReadOnlyPerformanceCounter Gen1Collections { get; }

		[PerformanceCounter("# Gen 2 Collections")]
		IReadOnlyPerformanceCounter Gen2Collections { get; }

		[PerformanceCounter("Allocated Bytes/sec")]
		IReadOnlyPerformanceCounter AllocatedBytesPerSecond { get; }

		[PerformanceCounter("% Time in GC")]
		IReadOnlyPerformanceCounter TimeInGC { get; }
	}
}