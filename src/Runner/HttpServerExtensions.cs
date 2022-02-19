using System.Net;

namespace HttpMock.Runner
{
	public static class HttpServerExtensions
	{
		/// <summary>
		/// Route GET request.
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path">e.g. /order</param>
		/// <param name="response"></param>
		public static void RouteGet(this HttpServer server, string path, Response response)
		{
			server.Route(HttpMethod.GET, path, response);
		}

		/// <summary>
		/// Route POST request.
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path">e.g. /order</param>
		/// <param name="response"></param>
		public static void RoutePost(this HttpServer server, string path, Response response)
		{
			server.Route(HttpMethod.POST, path, response);
		}

		/// <summary>
		/// Route PUT request.
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path">e.g. /order</param>
		/// <param name="response"></param>
		public static void RoutePut(this HttpServer server, string path, Response response)
		{
			server.Route(HttpMethod.PUT, path, response);
		}

		/// <summary>
		/// Route DELETE request.
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path">e.g. /order</param>
		/// <param name="response"></param>
		public static void RouteDelete(this HttpServer server, string path, Response response)
		{
			server.Route(HttpMethod.DELETE, path, response);
		}

		/// <summary>
		/// Run server on localhost (IP address: 127.0.0.1).
		/// </summary>
		/// <param name="server"></param>
		/// <param name="port">e.g. 80</param>
		public static void StartOnLocal(this HttpServer server, int port)
		{
			server.Start(IPAddress.Loopback, port);
		}
	}
}
