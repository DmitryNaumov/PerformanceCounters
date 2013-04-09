namespace PerformanceCounters
{
	using System;
	using System.Linq.Expressions;

	public static class PerformanceCounterSetExtensions
	{
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
	}
}