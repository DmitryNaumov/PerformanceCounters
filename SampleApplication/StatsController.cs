namespace SampleApplication
{
	using System;
	using System.Web.Http;

	public sealed class StatsController : ApiController
	{
		public Category[] GetMetadata()
		{
			return new[]
			{
				new Category()
				{
					Title = "System",
					Counters = new[]
					{
						new Counter
						{
							Name = "Process CPU Usage"
						},
						new Counter
						{
							Name = "Process Memory Usage"
						},
					}
				},
				new Category()
				{
					Title = "Application"
				},
			};
		}

		public Counter[] GetCounters()
		{
			return new []
			{
				new Counter
				{
					Name = "Process CPU Usage"
				},
				new Counter
				{
					Name = "Process Memory Usage"
				},
			};
		}

		public double GetCounterData()
		{
			return new Random().NextDouble();
		}

		public string Get()
		{
			return "Hello, JavaScript!";
		}
	}

	public class Category
	{
		public string Title { get; set; }
		public Counter[] Counters { get; set; }
	}

	public class Counter
	{
		public string Name { get; set; }
	}
}