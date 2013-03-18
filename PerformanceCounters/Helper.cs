namespace PerformanceCounters
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;

	internal static class Helper
	{
		internal static PerformanceCounterCategoryAttribute GetCategoryAttribute(Type type)
		{
			return (PerformanceCounterCategoryAttribute)type.GetCustomAttributes(typeof(PerformanceCounterCategoryAttribute), false).FirstOrDefault();
		}

		internal static CounterCreationData GetCounterCreationData(PropertyInfo propertyInfo)
		{
			var attribute = (PerformanceCounterAttribute)propertyInfo.GetCustomAttributes(typeof(PerformanceCounterAttribute), false).FirstOrDefault();
			if (attribute == null)
			{
				return null;
			}

			return new CounterCreationData(attribute.CounterName, attribute.CounterHelp, attribute.CounterType);
		}
	}
}
