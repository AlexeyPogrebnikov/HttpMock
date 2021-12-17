namespace HttpMock.Core
{
	public class Request
	{
		public string Method { get; set; }

		public string Path { get; set; }

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