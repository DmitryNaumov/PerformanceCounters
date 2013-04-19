namespace NeedfulThings.PerformanceCounters
{
	using System;
	using System.Diagnostics;

	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class PerformanceCounterAttribute : Attribute
	{
		private readonly string _counterName;
		private readonly string _counterHelp;
		private readonly PerformanceCounterType _counterType;

		public PerformanceCounterAttribute(string counterName, string counterHelp, PerformanceCounterType counterType)
		{
			_counterName = counterName;
			_counterHelp = counterHelp;
			_counterType = counterType;
		}

		public PerformanceCounterAttribute(string counterName)
		{
			_counterName = counterName;
		}

		public string CounterName
		{
			get { return _counterName; }
		}

		public string CounterHelp
		{
			get { return _counterHelp; }
		}

		public PerformanceCounterType CounterType
		{
			get { return _counterType; }
		}
	}
}