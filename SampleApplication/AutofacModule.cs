namespace SampleApplication
{
	using System.Linq;
	using System.Reflection;
	using System.Web.Http;
	using Autofac;
	using Autofac.Features.ResolveAnything;
	using Autofac.Integration.WebApi;
	using PerformanceCounters;

	internal sealed class AutofacModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterAssemblyTypes(
				Assembly.GetExecutingAssembly())
				.Where(t => !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t))
				.InstancePerMatchingLifetimeScope(AutofacWebApiDependencyResolver.ApiRequestTag);

			builder.Register(c => PerformanceCounterFactory.GetCounters<SampleCounters>()).AsImplementedInterfaces().AsSelf().SingleInstance();
			builder.Register(c => PerformanceCounterFactory.GetCounters<ClrMemoryCounters>()).AsImplementedInterfaces().AsSelf().SingleInstance();
			builder.Register(c => PerformanceCounterFactory.GetCounters<ClrJitCounters>()).AsImplementedInterfaces().AsSelf().SingleInstance();

			builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
		}
	}
}
