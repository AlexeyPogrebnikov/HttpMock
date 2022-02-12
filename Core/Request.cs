using System;
using System.Net.Sockets;
using System.Text;

namespace HttpMock.Core
{
	public class Request
	{
		public Request()
		{
			Time = DateTime.Now.TimeOfDay;
		}

		public TimeSpan Time { get; }

		public string Method { get; init; }

		public string Path { get; init; }

		public bool Handled { get; set; }

		public static Request Read(NetworkStream stream)
		{
			string content = GetRequestContent(stream);
			return Parse(content);
		}

		private static Request Parse(string s)
		{
			try
			{
				string[] parts = s.Split(" ");

				return new Request
				{
					Method = parts[0],
					Path = parts[1]
				};
			}
			catch (Exception e)
			{
				throw new InvalidOperationException($"Error a request parsing: {Environment.NewLine}{s}", e);
			}
		}

		private static string GetRequestContent(NetworkStream networkStream)
		{
			var buffer = new byte[1024];
			StringBuilder content = new();

			do
			{
				int count = networkStream.Read(buffer, 0, buffer.Length);
				content.Append(Encoding.Default.GetString(buffer, 0, count));
			} while (networkStream.DataAvailable);

			return content.ToString();
		}
	}
}