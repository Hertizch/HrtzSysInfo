using System.IO;
using System.Xml.Serialization;

namespace HrtzSysInfo.Extensions
{
    public static class XmlExtensions
    {
        public static void SerializeToXml<T>(T t, string filename)
        {
            var serializer = new XmlSerializer(t.GetType());
            using (TextWriter textWriter = new StreamWriter(filename))
                serializer.Serialize(textWriter, t);
        }

        public static T DeserializeFromXml<T>(string filename)
        {
            var deserializer = new XmlSerializer(typeof(T));

            T retVal;

            using (TextReader textReader = new StreamReader(filename))
                retVal = (T)deserializer.Deserialize(textReader);

            return retVal;
        }
    }
}
