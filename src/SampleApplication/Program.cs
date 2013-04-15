﻿namespace SampleApplication
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading;
	using System.Web.Http;
	using System.Web.Http.SelfHost;
	using Autofac.Integration.WebApi;
	using Autofac;

	class Program
	{
		static void Main(string[] args)
		{
			var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			var config = new HttpSelfHostConfiguration("http://localhost:2707/");
			config.MessageHandlers.Add(new StaticFileHandler(Path.Combine(rootPath, "web")));
			config.Routes.MapHttpRoute(
				name: "API Default",
				routeTemplate: "api/{controller}/{action}");

			var builder = new ContainerBuilder();
			builder.RegisterModule(new AutofacModule());

			var container = builder.Build();
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

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