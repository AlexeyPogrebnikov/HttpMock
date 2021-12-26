namespace HttpMock.Core
{
	public class Response
	{
		public string StatusCode { get; set; }

		public string Content { get; set; }

		public Response Clone()
		{
			return new Response
			{
				StatusCode = StatusCode,
				Content = Content
			};
		}
	}
}