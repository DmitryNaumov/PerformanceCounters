namespace SampleApplication
{
	using System.ComponentModel;
	using System.Configuration.Install;
	using NeedfulThings.PerformanceCounters;

	[RunInstaller(true)]
	public sealed class SampleCountersInstaller : Installer
	{
		public SampleCountersInstaller()
		{
			Installers.Add(PerformanceCounterFactory.GetInstallerFor<SampleCounters>());
		}
	}
}
