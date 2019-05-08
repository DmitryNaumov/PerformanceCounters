namespace NeedfulThings.PerformanceCounters
{
	using System.Diagnostics;

	public interface IPerformanceCounter : IReadOnlyPerformanceCounter
	{
		void Increment();

		void IncrementBy(long value);

		void Decrement();

		void Reset();
	}
}