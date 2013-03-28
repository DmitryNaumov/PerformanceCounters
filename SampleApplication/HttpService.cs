namespace SampleApplication
{
	using System.IO;
	using System.Net;

	internal sealed class HttpService
	{
		private readonly string _rootPath;
		private readonly HttpAsyncServer _httpServer = new HttpAsyncServer();

		public HttpService(string rootPath)
		{
			_rootPath = rootPath;
		}

		public void Start()
		{
			_httpServer.RequestReceived += RequestReceived;

			_httpServer.Start("http://*:2707/web/");
		}

		private void RequestReceived(HttpAsyncServer sender, HttpListenerContext context)
		{
			var request = context.Request;
			var response = context.Response;

			var path = Path.Combine(_rootPath, request.RawUrl.TrimStart('/'));

			if (System.String.Compare(request.RawUrl.TrimEnd('/'), "/web", System.StringComparison.OrdinalIgnoreCase) == 0)
			{
				response.Redirect("/web/index.html");
				response.Close();
				return;
			}

			if (!File.Exists(path))
			{
				response.StatusCode = (int)HttpStatusCode.NotFound;
				response.OutputStream.Close();
			}
			else
			{
				using (var writer = new StreamWriter(response.OutputStream))
				{
					using (var fileStream = File.OpenRead(path))
					{
						response.ContentType = GetContentType(path);
						response.ContentLength64 = fileStream.Length;
						response.StatusCode = (int)HttpStatusCode.OK;

						fileStream.CopyTo(response.OutputStream);
					}
				}
			}
		}

		private string GetContentType(string path)
		{
			var extension = Path.GetExtension(path);
			switch (extension)
			{
				case ".htm":
				case ".html":
					return "text/html";
				case ".css":
					return "text/css";
				case ".js":
					return "application/javascript";
			}

			return string.Empty;
		}
	}
}
