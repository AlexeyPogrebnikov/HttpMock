using HttpMock.Core;
using HttpMock.VisualServer.Commands;
using HttpMock.VisualServer.Model;
using Moq;
using NUnit.Framework;

namespace HttpMock.VisualServer.Tests.Commands
{
	[TestFixture]
	public class SaveRouteCommandTests
	{
		[SetUp]
		public void SetUp()
		{
			var httpServer = new Mock<IVisualHttpServer>();
			httpServer
				.SetupGet(server => server.Routes)
				.Returns(new RouteCollection());

			_routes = new RouteUICollection(httpServer.Object);
			_messageViewer = new Mock<IMessageViewer>();

			_command = new SaveRouteCommand(_routes, _messageViewer.Object)
			{
				MainWindowViewModel = new Mock<IMainWindowViewModel>().Object
			};
		}

		private SaveRouteCommand _command;
		private Mock<IMessageViewer> _messageViewer;
		private RouteUICollection _routes;

		[Test]
		public void Execute_view_error_if_Method_is_empty()
		{
			var route = new RouteUI
			{
				Method = string.Empty,
				Path = "/",
				Response = new ResponseUI()
			};

			_command.Execute(route);

			_messageViewer.Verify(viewer => viewer.View("", "Please fill required (*) fields."));
		}

		[Test]
		public void Execute_update_route()
		{
			var route = new RouteUI
			{
				Method = "GET",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 500
				}
			};

			var initialRoute = new RouteUI
			{
				Method = "GET",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			};
			_routes.Add(initialRoute);
			_command.InitialRoute = initialRoute;

			_command.Execute(route);

			_messageViewer.Verify(viewer => viewer.View(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}

		[Test]
		public void Execute_view_error_if_Path_is_empty()
		{
			var route = new RouteUI
			{
				Method = "POST",
				Path = string.Empty,
				Response = new ResponseUI()
			};

			_command.Execute(route);

			_messageViewer.Verify(viewer => viewer.View("", "Please fill required (*) fields."));
		}

		[Test]
		public void Execute_view_error_if_route_with_same_Method_and_Path_already_exists()
		{
			var route = new RouteUI
			{
				Method = "POST",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			};

			var initialRoute = new RouteUI
			{
				Method = "GET",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			};
			_routes.Add(initialRoute);
			_command.InitialRoute = initialRoute;

			_routes.Add(new RouteUI
			{
				Method = "POST",
				Path = "/",
				Response = new ResponseUI
				{
					StatusCode = 200
				}
			});

			_command.Execute(route);

			_messageViewer.Verify(viewer => viewer.View("", "A route with same Method and Path already exists."));
		}
	}
}