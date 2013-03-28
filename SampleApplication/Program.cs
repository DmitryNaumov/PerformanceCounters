namespace SampleApplication
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Net;
	using System.Reflection;
	using System.Text;
	using PerformanceCounters;

	class Program
	{
		static void Main(string[] args)
		{
			var sampleCounters = PerformanceCounterFactory.GetCounters<SampleCounters>();

			sampleCounters.RequestsTotalCount.Increment();

			var clrMemoryCounters = PerformanceCounterFactory.GetCounters<ClrMemoryCounters>();

			// TODO: sample app with blackjack and hookers

			var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			new HttpService(rootPath).Start();
			
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
