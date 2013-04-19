namespace NeedfulThings.PerformanceCounters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Castle.DynamicProxy;

	internal sealed class PerformanceCountersInterceptor<T> : IInterceptor where T : class, IPerformanceCounterSet
	{
		private readonly string _categoryName;
		private readonly IReadOnlyCollection<IPerformanceCounter> _counters;

		private readonly Dictionary<MethodInfo, IPerformanceCounter> _lookup =
			new Dictionary<MethodInfo, IPerformanceCounter>();

		public PerformanceCountersInterceptor()
		{
			var type = typeof (T);
			var categoryAttribute = Helper.GetCategoryAttribute(type);
			if (categoryAttribute == null)
			{
				throw new ArgumentException();
			}

			_categoryName = categoryAttribute.CategoryName;
			foreach (var propertyInfo in type.GetProperties())
			{
				var counterAttribute = GetCounterAttribute(propertyInfo);
				if (counterAttribute == null)
				{
					continue;
				}

				var getMethod = propertyInfo.GetGetMethod();
				if (getMethod == null)
				{
					throw new InvalidProgramException();
				}

				var counter = PerformanceCounterFactory.GetInstance(categoryAttribute, counterAttribute,
				                                                    propertyInfo.PropertyType ==
				                                                    typeof (IReadOnlyPerformanceCounter));
				_lookup.Add(getMethod, counter);

				var setMethod = propertyInfo.GetSetMethod();
				if (setMethod != null)
				{
					_lookup.Add(setMethod, counter);
				}
			}

			_counters = _lookup.OrderBy(kvp => kvp.Key.MetadataToken).Select(kvp => kvp.Value).ToList();
		}

		public void Intercept(IInvocation invocation)
		{
			if (invocation.Method == ReflectionHelper.CategoryNameProperty)
			{
				invocation.ReturnValue = _categoryName;
				return;
			}

			if (invocation.Method == ReflectionHelper.CountersProperty)
			{
				invocation.ReturnValue = _counters;
				return;
			}

			IPerformanceCounter counter;
			if (_lookup.TryGetValue(invocation.Method, out counter))
			{
				invocation.ReturnValue = counter;
			}
		}

		private PerformanceCounterAttribute GetCounterAttribute(PropertyInfo propertyInfo)
		{
			var attribute =
				(PerformanceCounterAttribute)
				propertyInfo.GetCustomAttributes(typeof (PerformanceCounterAttribute), false).FirstOrDefault();

			return attribute;
		}
	}

	internal static class ReflectionHelper
	{
		public static readonly MethodInfo CategoryNameProperty =
			typeof (IPerformanceCounterSet).GetProperty("CategoryName").GetGetMethod();

		public static readonly MethodInfo CountersProperty =
			typeof (IPerformanceCounterSet).GetProperty("Counters").GetGetMethod();
	}
}