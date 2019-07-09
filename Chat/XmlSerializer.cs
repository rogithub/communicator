using System.IO;
using Communicator;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using Communicator.Core;

namespace  Chat
{
    public class SerializationXml : IDataSerializer
    {

        private Stream ToStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(string xml)
        {
            T obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var stream = ToStream(xml))
            {
                XmlReader reader = XmlReader.Create(stream);
                obj = (T)serializer.Deserialize(reader);
            }

            return obj;
        }


        public string Serialize<T>(T cSharpObj)
        {
            StringBuilder sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = XmlWriter.Create(sb))
            {
                writer.WriteProcessingInstruction("xml", "version='1.0'");
                serializer.Serialize(writer, cSharpObj);
            }

            return sb.ToString();
        }
    }
}
