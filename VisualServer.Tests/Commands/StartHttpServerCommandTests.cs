using HttpMock.VisualServer.Commands;
using HttpMock.Core;
using Moq;
using NUnit.Framework;

namespace HttpMock.VisualServer.Tests.Commands
{
	[TestFixture]
	public class StartHttpServerCommandTests
	{
		[Test]
		public void Execute_view_warning_if_host_is_null()
		{
			var messageViewer = new Mock<IMessageViewer>();
			var command = new StartHttpServerCommand(new Mock<IVisualHttpServer>().Object, messageViewer.Object);

			var parameter = new ConnectionSettings
			{
				Host = null
			};

			command.Execute(parameter);

			messageViewer.Verify(viewer => viewer.View("Warning!", "Please enter a host."));
		}

		[Test]
		public void Execute_view_warning_if_port_is_null()
		{
			var messageViewer = new Mock<IMessageViewer>();
			var command = new StartHttpServerCommand(new Mock<IVisualHttpServer>().Object, messageViewer.Object);

			var parameter = new ConnectionSettings
			{
				Host = "127.0.0.1",
				Port = null
			};

			command.Execute(parameter);

			messageViewer.Verify(viewer => viewer.View("Warning!", "Please enter a port."));
		}

		[Test]
		public void Execute_view_warning_if_host_is_invalid()
		{
			var messageViewer = new Mock<IMessageViewer>();
			var command = new StartHttpServerCommand(new Mock<IVisualHttpServer>().Object, messageViewer.Object);

			var parameter = new ConnectionSettings
			{
				Host = "q"
			};

			command.Execute(parameter);

			messageViewer.Verify(viewer => viewer.View("Warning!", "The host 'q' is invalid."));
		}

		[Test]
		public void Execute_view_warning_if_port_is_invalid()
		{
			var messageViewer = new Mock<IMessageViewer>();
			var command = new StartHttpServerCommand(new Mock<IVisualHttpServer>().Object, messageViewer.Object);

			var parameter = new ConnectionSettings
			{
				Host = "127.0.0.1",
				Port = "ten"
			};

			command.Execute(parameter);

			messageViewer.Verify(viewer => viewer.View("Warning!", "The port 'ten' is invalid."));
		}
	}
}