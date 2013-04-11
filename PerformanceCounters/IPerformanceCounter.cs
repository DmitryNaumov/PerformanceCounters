namespace PerformanceCounters
{
	public interface IPerformanceCounter : IReadOnlyPerformanceCounter
	{
		string CounterName { get; }

		void Increment();

		void IncrementBy(long value);

		void Decrement();

		void Reset();
	}
}