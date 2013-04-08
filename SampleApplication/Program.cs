namespace SampleApplication
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading;
	using System.Web.Http;
	using System.Web.Http.SelfHost;
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
			//new HttpService(rootPath).Start();

			var config = new HttpSelfHostConfiguration("http://localhost:2707/");
			config.MessageHandlers.Add(new StaticFileHandler(Path.Combine(rootPath, "web")));
			config.Routes.MapHttpRoute(
				name: "API Default",
				routeTemplate: "api/{controller}/{action}",
				defaults: new { action = "get" });

			using (var server = new HttpSelfHostServer(config))
			{
				server.OpenAsync().Wait();

				Console.WriteLine("Press any key to exit...");

				var random = new Random();
				while (true)
				{
					var n = random.Next(100);
					while (n-- > 0)
					{
						if (Console.KeyAvailable)
							return;

						var dummyString = n.ToString();
					}

					Thread.Sleep(100);
				}
			}
		}
	}
}
