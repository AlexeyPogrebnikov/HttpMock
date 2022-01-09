namespace HttpMock.Core
{
	public class Response
	{
		public string StatusCode { get; set; }

		public string Body { get; set; }

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