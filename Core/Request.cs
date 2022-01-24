using System;

namespace HttpMock.Core
{
	public class Request
	{
		public Request()
		{
			Time = DateTime.Now.TimeOfDay;
			Handled = true;
		}

		public TimeSpan Time { get; }

		public string Method { get; init; }

		public string Path { get; init; }

		public bool Handled { get; internal set; }

		public static Request Parse(string s)
		{
			string[] parts = s.Split(" ");

			return new Request
			{
				Method = parts[0],
				Path = parts[1]
			};
		}
	}
}