using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace HttpMock.Runner
{
	[DataContract]
	internal class Connection
	{
		[DataMember] [XmlAttribute] internal string Host { get; set; }

		[DataMember] [XmlAttribute] internal int Port { get; set; }
	}
}