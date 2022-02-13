using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HttpMock.Core;
using Serilog;

namespace HttpMock.VisualServer
{
	public class VisualHttpServer : IVisualHttpServer
	{
		private readonly Route _defaultRoute = new()
		{
			Response = new Response
			{
				StatusCode = 404
			}
		};

		private bool _isStarted;
		private TcpListener _listener;
		private bool _startEnabled;
		private bool _stopEnabled;

		public VisualHttpServer()
		{
			Routes = new RouteCollection();
			Interactions = new InteractionCollection();
			StartEnabled = true;
		}

		public RouteCollection Routes { get; }
		public InteractionCollection Interactions { get; }
		public event EventHandler StatusChanged;

		public bool IsStarted
		{
			get => _isStarted;
			private set
			{
				_isStarted = value;
				OnStatusChanged();
			}
		}

		public bool StartEnabled
		{
			get => _startEnabled;
			private set
			{
				_startEnabled = value;
				OnStatusChanged();
			}
		}

		public bool StopEnabled
		{
			get => _stopEnabled;
			private set
			{
				_stopEnabled = value;
				OnStatusChanged();
			}
		}

		public async Task StartAsync(IPAddress address, int port)
		{
			if (!StartEnabled)
				throw new InvalidOperationException("HTTP server is already started.");

			StartEnabled = false;

			_listener = new TcpListener(address, port);
			_listener.Start();

			StopEnabled = true;
			IsStarted = true;
			Log.Information("HTTP server started.");

			try
			{
				await Task.Run(ProcessRequests);
			}
			catch (SocketException e)
			{
				if (e.SocketErrorCode != SocketError.Interrupted)
				{
					Log.Error(e, "Failed start the server or process a request.");
					throw;
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed start the server or process a request.");
				throw;
			}
			finally
			{
				IsStarted = false;
				StopEnabled = false;
				StartEnabled = true;
			}
		}

		public async Task StopAsync()
		{
			if (!StopEnabled)
				throw new InvalidOperationException("HTTP server is stopped.");

			StopEnabled = false;

			try
			{
				_listener.Stop();
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed stop the server.");
				throw;
			}

			while (IsStarted) await Task.Delay(100);

			Log.Information("HTTP server stopped.");

			StartEnabled = true;
		}

		private void ProcessRequests()
		{
			while (true)
			{
				TcpClient client = null;
				try
				{
					try
					{
						client = _listener.AcceptTcpClient();
					}
					catch (InvalidOperationException e)
					{
						Log.Warning(e, "Error accept a tcp client.");
						return;
					}

					using NetworkStream stream = client.GetStream();

					Request request;
					try
					{
						stream.ReadTimeout = 1000;
						request = Request.Read(stream);
					}
					catch (Exception e)
					{
						Log.Warning(e, "Error read a request.");
						continue;
					}

					Log.Information($"Process request {request.Method} {request.Path}");

					Route route = Routes.Find(request.Method, request.Path).FirstOrDefault();

					if (route == null)
					{
						request.Handled = false;
						route = _defaultRoute;
					}
					else
					{
						request.Handled = true;
					}

					Response response = route.Response;
					response.Write(stream);

					Interaction interaction = new()
					{
						Request = request,
						Response = response
					};

					Interactions.Add(interaction);
				}
				finally
				{
					client?.Dispose();
				}
			}
		}

		private void OnStatusChanged()
		{
			StatusChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}