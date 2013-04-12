namespace PerformanceCounters
{
	using System.Diagnostics;

	public interface IPerformanceCounter : IReadOnlyPerformanceCounter
	{
		string CounterName { get; }

		PerformanceCounterType CounterType { get; }

		void Increment();

		void IncrementBy(long value);

		void Decrement();

		void Reset();
	}
}