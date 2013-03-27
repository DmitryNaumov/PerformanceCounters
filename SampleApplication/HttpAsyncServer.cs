namespace SampleApplication
{
	using System;
	using System.Net;

	internal sealed class HttpAsyncServer
	{
		private readonly HttpListener _listener = new HttpListener();

		public event Action<HttpAsyncServer, HttpListenerContext> RequestReceived;

		public void Start(params string[] prefixes)
		{
			if (_listener.IsListening)
				throw new InvalidOperationException();

			foreach (var prefix in prefixes)
			{
				_listener.Prefixes.Add(prefix);
			}

			_listener.Start();

			Listen();
		}

		private void Listen()
		{
			_listener.GetContextAsync().ContinueWith(task => Accept(task.Result));
		}

		private void Accept(HttpListenerContext context)
		{
			Listen();

			var handler = RequestReceived;
			if (handler != null)
			{
				handler(this, context);
			}
		}
	}
}
