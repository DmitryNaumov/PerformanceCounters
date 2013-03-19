namespace SampleApplication
{
	using System;
	using PerformanceCounters;

	class Program
	{
		static void Main(string[] args)
		{
			var sampleCounters = PerformanceCounterFactory.GetCounters<SampleCounters>();

			sampleCounters.RequestsTotalCount.Increment();

			var clrMemoryCounters = PerformanceCounterFactory.GetCounters<ClrMemoryCounters>();

			int n = 1000000;
			while (n-- > 0)
			{
				var buffer = new char[16384];

				if (n % 10000 == 0)
				{
					Console.WriteLine(clrMemoryCounters.TimeInGC.NextValue());
				}
			}

			// TODO: sample app with blackjack and hookers
		}
	}
}
