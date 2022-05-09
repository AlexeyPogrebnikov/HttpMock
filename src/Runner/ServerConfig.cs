using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace HttpMock.Runner
{
	[DataContract]
	internal class ServerConfig
	{
		[DataMember] internal Connection Connection { get; set; }

		[DataMember] internal Route[] Routes { get; set; }

		internal void Save(string fileName)
		{
			var settings = new DataContractJsonSerializerSettings {UseSimpleDictionaryFormat = true};

			var serializer = new DataContractJsonSerializer(GetType(), settings);
			using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				using (XmlDictionaryWriter writer =
				       JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
				{
					serializer.WriteObject(writer, this);
					writer.Flush();
				}
			}
		}
	}
}