
using System.IO;
using System.Xml;
using Communicator.Core;
using System.Collections.Generic;

namespace Chat
{

    public static class MetaDataHelper
    {
		public static void SetFileInfo(this List<MetaData> md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.Set("fileName", info.Name);
			md.Set("ext", info.Extension);
		}
    } 
}
