using System.Text;

namespace HttpMock.Core
{
	internal class ResponseBuilder
	{
		// ReSharper disable InconsistentNaming
		private const string CRLF = "\r\n";
		// ReSharper restore InconsistentNaming

		private readonly Encoding _encoding;
		private string _statusCode;
		private string _body;

		public ResponseBuilder(Encoding encoding)
		{
			_encoding = encoding;
		}

		public void SetStatusCode(string statusCode)
		{
			_statusCode = statusCode;
		}

		public void SetBody(string body)
		{
			_body = body;
		}

		public byte[] Build()
		{
			var response = $"HTTP/1.1 {_statusCode} OK{CRLF}";
			string body = _body ?? string.Empty;
			response += $"Content-Length: {body.Length}{CRLF}{CRLF}";
			if (!string.IsNullOrEmpty(body))
				response += body + CRLF;

			return _encoding.GetBytes(response);
		}
	}
}