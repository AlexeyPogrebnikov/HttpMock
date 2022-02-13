namespace HttpMock.VisualServer.Model
{
	public class ResponseUI
	{
		public int StatusCode { get; init; }

		public string Body { get; init; }

		public ResponseUI Clone()
		{
			return new ResponseUI
			{
				StatusCode = StatusCode,
				Body = Body
			};
		}
	}
}