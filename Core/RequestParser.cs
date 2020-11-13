namespace HttpMock.Core
{
	public class RequestParser
	{
		public Request Parse(string s)
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