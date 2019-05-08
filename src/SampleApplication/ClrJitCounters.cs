using NeedfulThings.PerformanceCounters;

namespace SampleApplication
{
	using System.Diagnostics;

	[PerformanceCounterCategory(".NET CLR Jit", PerformanceCounterCategoryType.MultiInstance)]
	public interface ClrJitCounters : IPerformanceCounterSet
	{
		[PerformanceCounter("# of IL Bytes Jitted")]
		IReadOnlyPerformanceCounter ILBytesJitted { get; }

		[PerformanceCounter("# of Methods Jitted")]
		IReadOnlyPerformanceCounter MethodsJitted { get; }

		[PerformanceCounter("% Time in Jit")]
		IReadOnlyPerformanceCounter TimeInJit { get; }

		[PerformanceCounter("Total # of IL Bytes Jitted")]
		IReadOnlyPerformanceCounter TotalILBytesJitted { get; }
	}
}