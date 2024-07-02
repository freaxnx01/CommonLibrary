using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace Library
{
    public static class SerializationHelper
    {
        #region SerializeObjectToFile
        public static void SerializeObjectToFile(object obj, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }
        #endregion

        #region DeserializeObjectFromFile
        public static object DeserializeObjectFromFile(string file)
        {
            object returnValue = null;

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                returnValue = bf.Deserialize(fs);
            }

            return returnValue;
        }
        #endregion

        #region SerializeAsXml
        public static string SerializeAsXml(object o)
        {
            MemoryStream mem = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(o.GetType());
            XmlTextWriter writer = new XmlTextWriter(mem, Encoding.Default);
            ser.Serialize(writer, o);
            string xml = Encoding.Default.GetString(((MemoryStream)writer.BaseStream).ToArray());
            return xml;
        }
        #endregion

    }
}
