namespace PerformanceCounters
{
	using System.Diagnostics;

	internal sealed class NullPerformanceCounter : IPerformanceCounter
	{
		private readonly string _counterName;
		private readonly PerformanceCounterType _counterType;

		public NullPerformanceCounter(string counterName, PerformanceCounterType counterType)
		{
			_counterName = counterName;
			_counterType = counterType;
		}

		public string CounterName
		{
			get { return _counterName; }
		}

		public PerformanceCounterType CounterType
		{
			get { return _counterType; }
		}

		public void Increment()
		{
		}

		public void IncrementBy(long value)
		{
		}

		public void Decrement()
		{
		}

		public void Reset()
		{
		}

		public float NextValue()
		{
			return default(float);
		}

		public void Dispose()
		{
		}
	}
}