namespace TcpMock.Core
{
	public class RequestParser
	{
		public TcpInteraction Parse(string s)
		{
			string[] parts = s.Split(" ");

			return new TcpInteraction
			{
				Method = parts[0],
				Path = parts[1]
			};
		}
	}
}