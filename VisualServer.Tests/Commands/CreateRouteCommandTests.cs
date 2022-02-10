using HttpMock.Core;
using HttpMock.VisualServer.Commands;
using HttpMock.VisualServer.Model;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace HttpMock.VisualServer.Tests.Commands
{
	[TestFixture]
	public class CreateRouteCommandTests
	{
		private Mock<IHttpServer> _httpServer;

		[SetUp]
		public void SetUp()
		{
			_httpServer = new Mock<IHttpServer>();

			_httpServer.SetupGet(server => server.Routes).Returns(new RouteCollection());
		}

		[Test]
		public void Execute_view_error_if_route_with_same_Method_and_Path_already_added()
		{
			var messageViewer = new Mock<IMessageViewer>();
			RouteUICollection routes = new(_httpServer.Object)
			{
				new RouteUI
				{
					Method = "GET",
					Path = "/",
					Response = new ResponseUI
					{
						StatusCode = 200
					}
				}
			};

			CreateRouteCommand command = new(routes, messageViewer.Object);

			command.Execute(new RouteUI
			{
				Method = "GET",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			});

			messageViewer.Verify(viewer => viewer.View(string.Empty, "A route with same Method and Path already exists."));
		}

		[Test]
		public void Execute_add_route()
		{
			var messageViewer = new Mock<IMessageViewer>();
			RouteUICollection routes = new(_httpServer.Object)
			{
				new RouteUI
				{
					Method = "GET",
					Path = "/",
					Response = new ResponseUI
					{
						StatusCode = 200
					}
				}
			};

			CreateRouteCommand command = new(routes, messageViewer.Object);

			command.Execute(new RouteUI
			{
				Method = "GET",
				Path = "/foo",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			});

			Assert.AreEqual(2, routes.Count());
		}

		[Test]
		public void Execute_add_route_if_paths_are_same()
		{
			var messageViewer = new Mock<IMessageViewer>();
			RouteUICollection routes = new(_httpServer.Object)
			{
				new RouteUI
				{
					Method = "POST",
					Path = "/order",
					Response = new ResponseUI
					{
						StatusCode = 200
					}
				}
			};

			CreateRouteCommand command = new(routes, messageViewer.Object);

			command.Execute(new RouteUI
			{
				Method = "GET",
				Path = "/order",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			});

			Assert.AreEqual(2, routes.Count());
		}
	}
}
