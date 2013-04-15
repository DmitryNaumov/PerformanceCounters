namespace SampleApplication
{
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading;
	using System.Threading.Tasks;

	internal sealed class StaticFileHandler : DelegatingHandler
	{
		private readonly string _rootPath;

		public StaticFileHandler(string rootPath)
		{
			_rootPath = rootPath;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var path = Path.Combine(_rootPath, request.RequestUri.LocalPath.TrimStart('/'));

			if (request.RequestUri.LocalPath == "/")
			{
				path = Path.Combine(_rootPath, "index.html");
			}

			if (!File.Exists(path))
			{
				return base.SendAsync(request, cancellationToken);
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