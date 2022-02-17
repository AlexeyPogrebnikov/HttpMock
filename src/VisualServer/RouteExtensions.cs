using HttpMock.Core;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer
{
	internal static class RouteExtensions
	{
		internal static RouteUI Convert(this Route route)
		{
			return new RouteUI
			{
				Method = route.Method,
				Path = route.Path,
				Response = new ResponseUI
				{
					StatusCode = route.Response.StatusCode,
					Body = route.Response.Body
				}
			};
		}
	}
}
