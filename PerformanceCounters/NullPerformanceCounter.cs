namespace PerformanceCounters
{
	internal sealed class NullPerformanceCounter : IPerformanceCounter
	{
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

		public void Dispose()
		{
		}
	}
}