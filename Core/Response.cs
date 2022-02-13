using System.IO;
using System.Text;

namespace HttpMock.Core
{
	public class Response
	{
		public int StatusCode { get; init; }

		public string Body { get; init; }

		public void Write(Stream stream)
		{
			string responseText = GetResponseText();
			byte[] data = Encoding.Default.GetBytes(responseText);
			stream.Write(data, 0, data.Length);
		}

		private string GetResponseText()
		{
			const string CRLF = "\r\n";

			StringBuilder sb = new($"HTTP/1.1 {StatusCode} OK{CRLF}");
			string body = Body ?? string.Empty;
			sb.Append($"Content-Length: {body.Length}{CRLF}{CRLF}");
			if (!string.IsNullOrEmpty(body))
				sb.Append(body + CRLF);

			return sb.ToString();
		}

	}
}