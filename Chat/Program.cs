using System;
using Communicator;
using System.IO;
using System.Xml;

namespace Chat
{
	class Program
	{
		public static void PrintHelp()
		{
			Console.Error.WriteLine("Chat [username]");
		}

		public static MetaData GetChatMetaData(string userName)
		{
			MetaData md = new MetaData();
			md.SetValueString("user", userName);
			return md;
		}

		public static MetaData GetFileMetaData(string userName, string path)
		{

			FileInfo info = new FileInfo(path);

			MetaData md = new MetaData();
			md.SetValueString("user", userName);
			md.SetValueString("fileName", info.Name);
			md.SetValueString("ext", info.Extension);

			return md;
		}

		public static MetaData GetXmlMetaData(string userName, XmlDocument doc)
		{
			MetaData md = new MetaData();
			md.SetValueString("user", userName);
			md.SetValueString("content", doc.OuterXml);			
			return md;
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				PrintHelp();
				return;
			}

			string userName = args[0];
			Console.WriteLine($"Welcome: {userName}");

			string url = "http://localhost:5000/communicator";
			EventSource source = new EventSource(url);
			

			source.Handle.String("Chat", (md, data) =>
			{
				string user = md.GetValueString("user");
				Console.WriteLine($"[{user}]: {data}");
			});

			source.Handle.Binary("File", (md, data) =>
			{
				string user = md.GetValueString("user");
				string fileName = md.GetValueString("fileName");				
				Console.WriteLine($"[{user}]: {fileName} length {data.Length}");
			});

			source.Handle.Xml("Xml", (md, data) =>
			{
				string user = md.GetValueString("user");
				string content = md.GetValueString("content");
				Console.WriteLine($"[{user}]: Xml \n\r {content}");
			});

			string message = "Connected";
			string path = string.Empty;
			bool exit = false;

			while (!exit)
			{
				switch (message)
				{
					case "quit":
						exit = true;
						break;
					case "file":
						Console.WriteLine("Enter path:");
						path = Console.ReadLine();
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							source.Raise.Binary("File", bytes, GetFileMetaData(userName, path));
						}
						break;
					case "xml":
						Console.WriteLine("Enter path:");
						path = Console.ReadLine();
						if (File.Exists(path))
						{
							XmlDocument doc = new XmlDocument();
							string contents = File.ReadAllText(path);
							doc.LoadXml(contents);
							source.Raise.Xml("Xml", doc, GetXmlMetaData(userName, doc));
						}
						break;
					default:
						source.Raise.String("Chat", message, GetChatMetaData(userName));
						break;
				}

				message = Console.ReadLine();
			}

			source.Raise.String("Chat", "Disconnected", GetChatMetaData(userName));
		}
	}
}