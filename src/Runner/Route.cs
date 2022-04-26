using System.Runtime.Serialization;

namespace HttpMock.Runner
{
	[DataContract]
	internal class Route
	{
		[DataMember] internal string Method { get; set; }

		[DataMember] internal string Path { get; set; }

		[DataMember] internal Response Response { get; set; }
	}
}