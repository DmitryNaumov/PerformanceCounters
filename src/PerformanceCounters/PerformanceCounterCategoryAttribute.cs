namespace NeedfulThings.PerformanceCounters
{
	using System;
	using System.Diagnostics;

	[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
	public sealed class PerformanceCounterCategoryAttribute : Attribute
	{
		private readonly string _categoryName;
		private readonly string _categoryHelp;
		private readonly PerformanceCounterCategoryType _categoryType;

		public PerformanceCounterCategoryAttribute(string categoryName, string categoryHelp,
		                                           PerformanceCounterCategoryType categoryType)
		{
			_categoryName = categoryName;
			_categoryHelp = categoryHelp;
			_categoryType = categoryType;
		}

		public PerformanceCounterCategoryAttribute(string categoryName, PerformanceCounterCategoryType categoryType)
		{
			_categoryName = categoryName;
			_categoryType = categoryType;
		}

		public string CategoryName
		{
			get { return _categoryName; }
		}

		public string CategoryHelp
		{
			get { return _categoryHelp; }
		}

		public PerformanceCounterCategoryType CategoryType
		{
			get { return _categoryType; }
		}
	}
}