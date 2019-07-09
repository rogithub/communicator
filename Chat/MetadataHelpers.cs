
using System.IO;
using System.Xml;
using Communicator.Core;
using System.Collections.Generic;

namespace Chat
{

    public static class MetaDataHelper
    {
        public static void SetId(this List<MetaData> md, string id)
		{						
			md.Set("id", id);
		}

		public static void SetUser(this List<MetaData> md, string userName)
		{
			md.Set("user", userName);
		}

		public static void SetTo(this List<MetaData> md, string to)
		{
			md.Set("to", to);
		}

		public static void SetFileInfo(this List<MetaData> md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.Set("fileName", info.Name);
			md.Set("ext", info.Extension);
		}

		public static void SetXmlData(this List<MetaData> md, XmlDocument doc)
		{
			md.Set("content", doc.OuterXml);
		}

    } 
}
