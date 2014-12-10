using System.IO;
using System.Web.Http;

namespace NeedfulThings.PerformanceCounters.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static void UsePerformanceCounters(this HttpConfiguration configuration, string route)
        {
            var rootPath = Path.GetDirectoryName(typeof(HttpConfigurationExtensions).Assembly.Location);

            configuration.MessageHandlers.Add(new StaticFileHandler(Path.Combine(rootPath, "web"), route));
            configuration.Routes.MapHttpRoute(
                name: "Performance Counters",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { controller = "stats" }
                );
        }
    }
}
