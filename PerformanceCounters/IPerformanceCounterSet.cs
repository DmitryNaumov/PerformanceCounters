namespace PerformanceCounters
{
	using System.Diagnostics;

	public interface IPerformanceCounterSet
	{
		string CategoryName { get; }
		
		string CategoryHelp { get; }

		PerformanceCounterCategoryType CategoryType { get; }
	}
}
