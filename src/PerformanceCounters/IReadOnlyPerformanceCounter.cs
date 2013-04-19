namespace NeedfulThings.PerformanceCounters
{
	using System;

	public interface IReadOnlyPerformanceCounter : IDisposable
	{
		float NextValue();
	}
}