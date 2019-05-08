namespace NeedfulThings.PerformanceCounters
{
	using System.Collections.Generic;

	public interface IPerformanceCounterSet
	{
		string CategoryName { get; }

		IReadOnlyCollection<IReadOnlyPerformanceCounter> Counters { get; }
	}
}