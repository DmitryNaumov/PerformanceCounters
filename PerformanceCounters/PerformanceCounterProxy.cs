namespace PerformanceCounters
{
	using System;
	using System.Diagnostics;

	internal sealed class PerformanceCounterProxy : IPerformanceCounter
	{
		private readonly PerformanceCounter _perfCounter;

		public PerformanceCounterProxy(PerformanceCounter perfCounter)
		{
			if (perfCounter == null)
			{
				throw new ArgumentNullException("perfCounter");
			}

			_perfCounter = perfCounter;
		}

		public void Increment()
		{
			_perfCounter.Increment();
		}

		public void IncrementBy(long value)
		{
			_perfCounter.IncrementBy(value);
		}

		public void Decrement()
		{
			_perfCounter.Decrement();
		}

		public void Reset()
		{
			_perfCounter.RawValue = 0;
		}

		public float NextValue()
		{
			return _perfCounter.NextValue();
		}

		void IDisposable.Dispose()
		{
			_perfCounter.Dispose();
		}
	}
}