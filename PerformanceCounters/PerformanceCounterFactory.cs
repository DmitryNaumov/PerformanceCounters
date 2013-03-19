namespace PerformanceCounters
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Castle.DynamicProxy;

	public static class PerformanceCounterFactory
	{
		private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
		private static readonly NullPerformanceCounter NullInstance = new NullPerformanceCounter();

		private static readonly Dictionary<Type, IPerformanceCounterSet> _counters = new Dictionary<Type, IPerformanceCounterSet>();

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
			lock (_counters)
			{
				IPerformanceCounterSet counterSet;
				if (!_counters.TryGetValue(typeof(T), out counterSet))
				{
					counterSet = ProxyGenerator.CreateInterfaceProxyWithoutTarget<T>(new PerformanceCountersInterceptor<T>());
					_counters.Add(typeof(T), counterSet);
				}

				return (T) counterSet;
			}
		}

		internal static IPerformanceCounter GetInstance(string categoryName, string counterName, PerformanceCounterCategoryType categoryType, bool readOnly)
		{
			try
			{
				if (PerformanceCounterCategory.Exists(categoryName) && PerformanceCounterCategory.CounterExists(counterName, categoryName))
				{
					var instanceName = categoryType == PerformanceCounterCategoryType.SingleInstance ? string.Empty : Process.GetCurrentProcess().ProcessName;

					var counter = new PerformanceCounter(categoryName, counterName, instanceName, readOnly);
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