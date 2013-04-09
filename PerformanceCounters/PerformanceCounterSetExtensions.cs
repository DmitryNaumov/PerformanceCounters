namespace PerformanceCounters
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

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

		public static string GetCounterName<T, TResult>(this T counterSet, Expression<Func<T, TResult>> selector) where T : IPerformanceCounterSet
		{
			var expression = selector.Body as MemberExpression;
			if (expression == null)
			{
				throw new ArgumentException("selector");
			}

			var attributes = expression.Member.GetCustomAttributes(typeof (PerformanceCounterAttribute), false);
			if (attributes.Length != 1)
			{
				throw new ArgumentException("counterSet");
			}

			var counterAttribute = (PerformanceCounterAttribute)attributes[0];

			return counterAttribute.CounterName;
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