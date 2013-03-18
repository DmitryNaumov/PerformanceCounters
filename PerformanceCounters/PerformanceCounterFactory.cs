namespace PerformanceCounters
{
	using System;
	using System.Diagnostics;
	using Castle.DynamicProxy;

	public sealed class PerformanceCounterFactory
	{
		private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
		private static readonly NullPerformanceCounter NullInstance = new NullPerformanceCounter();

		public static PerformanceCounterInstaller GetInstallerFor<T>() where T : IPerformanceCounterSet
		{
			var category = Helper.GetCategoryAttribute(typeof(T));
			if (category == null)
			{
				var message = string.Format("Type '{0}' should be marked with PerformanceCounterCategoryAttribute", typeof(T));
				throw new ArgumentException(message);
			}

			var installer = new PerformanceCounterInstaller
				{
					CategoryName = category.CategoryName,
					CategoryHelp = category.CategoryHelp,
					CategoryType = category.CategoryType
				};

			foreach (var propertyInfo in typeof(T).GetProperties())
			{
				var counterCreationData = Helper.GetCounterCreationData(propertyInfo);
				if (counterCreationData == null)
					continue;

				installer.Counters.Add(counterCreationData);
			}

			return installer;
		}

		public static T GetCounters<T>() where T : class, IPerformanceCounterSet
		{
			return ProxyGenerator.CreateInterfaceProxyWithoutTarget<T>(new PerformanceCountersInterceptor<T>());
		}

		internal static IPerformanceCounter GetInstance(string categoryName, string counterName)
		{
			try
			{
				if (PerformanceCounterCategory.Exists(categoryName) && PerformanceCounterCategory.CounterExists(counterName, categoryName))
				{
					var counter = new PerformanceCounter(categoryName, counterName, false);
					return new PerformanceCounterProxy(counter);
				}
			}
			catch
			{
			}

			return NullInstance;
		}
	}
}