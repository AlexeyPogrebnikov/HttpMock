namespace HttpMock.Core
{
	public class Route
	{
		public string Method { get; init; }

		public string Path { get; init; }

		public Response Response { get; init; }
	}
}