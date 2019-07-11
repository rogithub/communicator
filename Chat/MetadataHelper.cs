using System.IO;
using Communicator.Core;
using System.Collections.Generic;

namespace Chat
{

    public static class MetadataHelper
    {
		public static void SetFileInfo(this List<KeyValue> md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.Set("fileName", info.Name);
			md.Set("ext", info.Extension);
		}
    } 
}