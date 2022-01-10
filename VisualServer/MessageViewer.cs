using System.Windows;

namespace HttpMock.VisualServer
{
	public class MessageViewer : IMessageViewer
	{
		public void View(string caption, string text)
		{
			MessageBox.Show(text, caption);
		}
	}
}