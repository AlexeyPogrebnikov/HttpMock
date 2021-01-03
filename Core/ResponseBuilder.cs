using System.Text;

namespace HttpMock.Core
{
	public class ResponseBuilder
	{
		// ReSharper disable InconsistentNaming
		private const string CRLF = "\r\n";
		// ReSharper restore InconsistentNaming

		private readonly Encoding _encoding;
		private string _statusCode;
		private string _content;

		public ResponseBuilder(Encoding encoding)
		{
			_encoding = encoding;
		}

		public void SetStatusCode(string statusCode)
		{
			_statusCode = statusCode;
		}

		public void SetContent(string content)
		{
			_content = content;
		}

		public byte[] Build()
		{
			var response = $"HTTP/1.1 {_statusCode} OK{CRLF}";
			string content = _content ?? string.Empty;
			response += $"Content-Length: {content.Length}{CRLF}{CRLF}";
			if (!string.IsNullOrEmpty(content))
				response += content + CRLF;

			return _encoding.GetBytes(response);
		}
	}
}