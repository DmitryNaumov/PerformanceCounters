namespace NeedfulThings.PerformanceCounters
{
	using System;
	using System.Diagnostics;

	internal sealed class PerformanceCounterProxy : IPerformanceCounter
	{
		private readonly PerformanceCounter _counter;

		public PerformanceCounterProxy(PerformanceCounter counter)
		{
			if (counter == null)
			{
				throw new ArgumentNullException("counter");
			}

			_counter = counter;
		}

		public string CounterName
		{
			get { return _counter.CounterName; }
		}

		public PerformanceCounterType CounterType
		{
			get { return _counter.CounterType; }
		}

		public void Increment()
		{
			_counter.Increment();
		}

		public void IncrementBy(long value)
		{
			_counter.IncrementBy(value);
		}

		public void Decrement()
		{
			_counter.Decrement();
		}

		public void Reset()
		{
			_counter.RawValue = 0;
		}

		public float NextValue()
		{
			return _counter.NextValue();
		}

		void IDisposable.Dispose()
		{
			_counter.Dispose();
		}
	}
}