using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NeedfulThings.PerformanceCounters.WebApi
{
    internal sealed class StaticFileHandler : DelegatingHandler
	{
		private readonly string _baseDirectory;
        private readonly string _route;

		public StaticFileHandler(string baseDirectory, string route)
		{
			_baseDirectory = baseDirectory;

		    if (route.StartsWith("/"))
		        _route = route;
		    else
		        _route = "/" + route;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
            if (!request.RequestUri.LocalPath.StartsWith(_route))
            {
                return base.SendAsync(request, cancellationToken);
            }

		    if (request.RequestUri.LocalPath == _route)
		    {
		        var redirect = request.CreateResponse(HttpStatusCode.Redirect);
		        redirect.Headers.Location = new Uri(request.RequestUri + "/");
		        return Task.FromResult(redirect);
		    }

		    string path;
		    if (request.RequestUri.LocalPath.Substring(_route.Length) == "/")
		    {
		        path = Path.Combine(_baseDirectory, "index.html");
		    }
		    else
		    {
		        path = Path.Combine(_baseDirectory, request.RequestUri.LocalPath.Substring(_route.Length).TrimStart('/'));
		    }

		    if (!File.Exists(path))
            {
                return Task.FromResult(request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found"));
            }

			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StreamContent(File.OpenRead(path));
			response.Content.Headers.ContentType = GetContentType(path);

			return Task.FromResult(response);
		}

		private MediaTypeHeaderValue GetContentType(string path)
		{
			var extension = Path.GetExtension(path);
			switch (extension)
			{
				case ".htm":
				case ".html":
					return new MediaTypeHeaderValue("text/html");
				case ".css":
					return new MediaTypeHeaderValue("text/css");
				case ".js":
					return new MediaTypeHeaderValue("application/javascript");
			}

			return null;
		}
	}
}