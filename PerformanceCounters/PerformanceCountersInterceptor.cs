namespace PerformanceCounters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Castle.DynamicProxy;

	internal sealed class PerformanceCountersInterceptor<T> : IInterceptor where T : class, IPerformanceCounterSet
	{
		private static readonly MethodInfo CategoryName = typeof (IPerformanceCounterSet).GetProperty("CategoryName").GetGetMethod();

		private readonly string _categoryName;
		private readonly Dictionary<MethodInfo, IPerformanceCounter> _counters = new Dictionary<MethodInfo, IPerformanceCounter>();

		public PerformanceCountersInterceptor()
		{
			var type = typeof(T);
			var attribute = Helper.GetCategoryAttribute(type);
			if (attribute == null)
			{
				throw new ArgumentException();
			}

			_categoryName = attribute.CategoryName;
			foreach (var propertyInfo in type.GetProperties())
			{
				var counterName = GetCounterName(propertyInfo);
				if (string.IsNullOrEmpty(counterName))
				{
					continue;
				}

				var getMethod = propertyInfo.GetGetMethod();
				if (getMethod == null)
				{
					throw new InvalidProgramException();
				}

				var counter = PerformanceCounterFactory.GetInstance(_categoryName, counterName, attribute.CategoryType, propertyInfo.PropertyType == typeof(IReadOnlyPerformanceCounter));
				_counters.Add(getMethod, counter);

				var setMethod = propertyInfo.GetSetMethod();
				if (setMethod != null)
				{
					_counters.Add(setMethod, counter);
				}
			}
		}

		public void Intercept(IInvocation invocation)
		{
			if (invocation.Method == CategoryName)
			{
				invocation.ReturnValue = _categoryName;
				return;
			}

			IPerformanceCounter counter;
			if (_counters.TryGetValue(invocation.Method, out counter))
			{
				invocation.ReturnValue = counter;
			}
		}

		private string GetCounterName(PropertyInfo propertyInfo)
		{
			var attribute = (PerformanceCounterAttribute)propertyInfo.GetCustomAttributes(typeof(PerformanceCounterAttribute), false).FirstOrDefault();
			if (attribute == null)
			{
				return string.Empty;
			}

			return attribute.CounterName;
		}
	}
}