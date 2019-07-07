
using System.IO;
using System.Xml;
using Communicator;

namespace Chat
{

    public static class MetaDataHelper
    {
        public static void SetId(this MetaData md, string id)
		{
			md.SetValueString("id", id);
		}

		public static void SetUser(this MetaData md, string userName)
		{
			md.SetValueString("user", userName);
		}

		public static void SetTo(this MetaData md, string to)
		{
			md.SetValueString("to", to);
		}

		public static void SetFileInfo(this MetaData md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.SetValueString("fileName", info.Name);
			md.SetValueString("ext", info.Extension);
		}

		public static void SetXmlData(this MetaData md, XmlDocument doc)
		{
			md.SetValueString("content", doc.OuterXml);
		}

    } 
}