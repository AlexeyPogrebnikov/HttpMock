namespace HttpMock.Core
{
	public class Response
	{
		public int StatusCode { get; init; }

		public string Body { get; init; }

		public Response Clone()
		{
			return new Response
			{
				StatusCode = StatusCode,
				Body = Body
			};
		}
	}
}