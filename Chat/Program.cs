using System;
using Communicator;
using System.IO;
using System.Xml;
using System.Collections.Generic;

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

		public static MetaData GetChatToMetaData(string userName, string to)
		{
			MetaData md = new MetaData();
			md.SetValueString("user", userName);
			md.SetValueString("to", to);
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

		private static Dictionary<string, string> Users = new Dictionary<string, string>();
		private static void AddUser(string connectionId, string userName)
		{			
			if (!Users.ContainsKey(userName)) {					
				Users.Add(userName, connectionId);
			}
			Users[userName] = connectionId;
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				PrintHelp();
				return;
			}

			string userName = args[0];
			

			string url = "http://localhost:5000/communicator";
			EventSource source = new EventSource(url);			
			Console.WriteLine($"Welcome: {userName}");

			source.Connect(GetChatMetaData(userName)).GetAwaiter().GetResult();
			
			source.Handle.OnConnected((md, connectionId)=> {
				string user = md.GetValueString("user");				
				AddUser(connectionId, user);
				Console.WriteLine($"{user}: {connectionId}");
			});

			source.Handle.String("Chat", (md, data) =>
			{
				string user = md.GetValueString("user");
				Console.WriteLine($"[{user}]: {data}");
			});

			source.Handle.String("ChatTo", (md, data) =>
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
			string to = string.Empty;
			bool exit = false;

			while (!exit)
			{
				switch (message)
				{
					case "quit":
						source.Raise.String("Chat", "Disconnected", GetChatMetaData(userName));
						return;
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
					case "to":
						string user = Console.ReadLine();
						to = string.Empty;
						Users.TryGetValue(user, out to);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = Console.ReadLine();							
							source.Raise.StringTo("ChatTo", to, message, GetChatToMetaData(userName, to));
						}else
						{
							Console.WriteLine($"User not found: {user}");
						}
					break;
					default:
						source.Raise.String("Chat", message, GetChatMetaData(userName));
						break;
				}

				message = Console.ReadLine();
			}
		}
	}
}