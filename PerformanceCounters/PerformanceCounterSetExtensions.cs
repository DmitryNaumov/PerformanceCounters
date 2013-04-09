namespace PerformanceCounters
{
	using System;
	using System.Linq;

	public static class PerformanceCounterSetExtensions
	{
		public static string GetCategoryName(this IPerformanceCounterSet counterSet)
		{
			var attributes = counterSet.GetType().GetCustomAttributes(typeof (PerformanceCounterCategoryAttribute), false);
			if (attributes.Length != 1)
			{
				throw new ArgumentException("counterSet");
			}

			var categoryAttribute = (PerformanceCounterCategoryAttribute)attributes[0];
			return categoryAttribute.CategoryName;
		}

		public static string[] GetCounterNames(this IPerformanceCounterSet counterSet)
		{
			return
				counterSet.GetType()
					.GetProperties()
					.Where(pi => typeof (IReadOnlyPerformanceCounter).IsAssignableFrom(pi.PropertyType))
					.OrderBy(pi => pi.MetadataToken)
					.Select(pi => Helper.GetCounterCreationData(pi).CounterName)
					.ToArray();
		}

		public static float[] GetCounterValues(this IPerformanceCounterSet counterSet)
		{
			return
				counterSet.GetType()
					.GetProperties()
					.Where(pi => typeof(IReadOnlyPerformanceCounter).IsAssignableFrom(pi.PropertyType))
					.OrderBy(pi => pi.MetadataToken)
					.Select(pi => ((IReadOnlyPerformanceCounter)pi.GetValue(counterSet)).NextValue())
					.ToArray();
		}
	}
}