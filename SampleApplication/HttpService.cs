namespace SampleApplication
{
	using System.IO;
	using System.Net;
	using System.Reflection;
	using System.Text;

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
				path = Path.Combine(_rootPath, @"web/index.html");
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
					var content = File.ReadAllText(path);
					writer.Write(content);

					response.ContentType = "text/html";
					response.ContentLength64 = Encoding.UTF8.GetByteCount(content);
					response.StatusCode = (int) HttpStatusCode.OK;
				}
			}
		}
	}
}
