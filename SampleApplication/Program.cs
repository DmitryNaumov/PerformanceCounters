namespace SampleApplication
{
	using PerformanceCounters;

	class Program
	{
		static void Main(string[] args)
		{
			var counters = PerformanceCounterFactory.GetCounters<SampleCounters>();

			counters.RequestsTotalCount.Increment();

			// TODO: sample app with blackjack and hookers
		}
	}
}
