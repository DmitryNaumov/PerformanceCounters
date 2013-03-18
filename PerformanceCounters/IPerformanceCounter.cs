namespace PerformanceCounters
{
	using System;

	public interface IPerformanceCounter : IDisposable
	{
		void Increment();

		void IncrementBy(long value);

		void Decrement();

		void Reset();
	}
}