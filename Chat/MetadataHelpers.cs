
using System.IO;
using System.Xml;
using Communicator;

namespace Chat
{

    public static class MetaDataHelper
    {
        public static void SetId(this MetaData md, string id)
		{
			md.Set("id", id);
		}

		public static void SetUser(this MetaData md, string userName)
		{
			md.Set("user", userName);
		}

		public static void SetTo(this MetaData md, string to)
		{
			md.Set("to", to);
		}

		public static void SetFileInfo(this MetaData md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.Set("fileName", info.Name);
			md.Set("ext", info.Extension);
		}

		public static void SetXmlData(this MetaData md, XmlDocument doc)
		{
			md.Set("content", doc.OuterXml);
		}

    } 
}
