using System.Runtime.Serialization;

namespace HttpMock.Runner
{
	[DataContract]
	public class Response
	{
		[DataMember] public int StatusCode { get; set; }

		[DataMember] public string Body { get; set; }
	}
}