using System.Diagnostics;

namespace NeedfulThings.PerformanceCounters
{
	using System;

	public interface IReadOnlyPerformanceCounter : IDisposable
	{
		string CounterName { get; }

		PerformanceCounterType CounterType { get; }
		
		float NextValue();
	}
}