using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace HttpMock.Runner
{
	public class HttpServer : IDisposable
	{
		private readonly string _serverFileName;
		private readonly ServerValidator _serverValidator;
		Process _serverProcess;
		private readonly List<Route> _routes = new List<Route>();
		private bool _disposedValue;

		/// <param name="serverFileName">e.g. "C:\HttpMockServer\HttpMock.Server.exe", You can download the server here https://github.com/AlexeyPogrebnikov/HttpMock/releases </param>
		public HttpServer(string serverFileName)
			: this(serverFileName, new ServerValidator(new VersionService()))
		{
		}

		internal HttpServer(string serverFileName, ServerValidator serverValidator)
		{
			_serverFileName = serverFileName;
			_serverValidator = serverValidator;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="method">HTTP defines a set of request methods to indicate the desired action to be performed for a given resource.</param>
		/// <param name="path">e.g. /year</param>
		/// <param name="response"></param>
		public void Route(HttpMethod method, string path, Response response)
		{
			_routes.Add(new Route
			{
				Method = method.ToString(),
				Path = path,
				Response = response
			});
		}

		/// <summary>
		/// Start the server on host and port.
		/// </summary>
		/// <param name="host">e.g. 127.0.0.1</param>
		/// <param name="port">e.g. 5000</param>
		/// <exception cref="FileNotFoundException">The server file not found.</exception>
		/// <exception cref="InvalidOperationException">The library version does not match the server version.</exception>
		public void Start(IPAddress host, int port)
		{
			Start(host.ToString(), port);
		}

		/// <summary>
		/// Start the server on host and port.
		/// </summary>
		/// <param name="host">e.g. 127.0.0.1</param>
		/// <param name="port">e.g. 5000</param>
		/// <exception cref="FileNotFoundException">The server file not found.</exception>
		/// <exception cref="InvalidOperationException">The library version does not match the server version.</exception>
		public void Start(string host, int port)
		{
			_serverValidator.Validate(_serverFileName);

			string configPath = Path.GetTempFileName();

			ServerConfig config = new ServerConfig
			{
				Connection = new Connection
				{
					Host = host,
					Port = port
				},
				Routes = _routes.ToArray()
			};

			config.Save(configPath);

			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = _serverFileName,
				Arguments = configPath
			};

			_serverProcess = Process.Start(startInfo);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				if (_serverProcess != null)
					_serverProcess.Dispose();
				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_disposedValue = true;
			}
		}

		~HttpServer()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
