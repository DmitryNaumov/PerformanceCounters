namespace PerformanceCounters
{
	internal sealed class NullPerformanceCounter : IPerformanceCounter
	{
		private readonly string _counterName;

		public NullPerformanceCounter(string counterName)
		{
			_counterName = counterName;
		}

		public string CounterName
		{
			get { return _counterName; }
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