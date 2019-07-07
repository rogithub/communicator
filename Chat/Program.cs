using System;
using System.Linq;
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

		public static void SetId(MetaData md, string id)
		{
			md.SetValueString("id", id);
		}

		public static void SetUser(MetaData md, string userName)
		{
			md.SetValueString("user", userName);
		}

		public static void SetTo(MetaData md, string to)
		{
			md.SetValueString("to", to);
		}

		public static void SetFileInfo(MetaData md, string path)
		{
			FileInfo info = new FileInfo(path);
			md.SetValueString("fileName", info.Name);
			md.SetValueString("ext", info.Extension);
		}

		public static void SetXmlData(MetaData md, XmlDocument doc)
		{
			md.SetValueString("content", doc.OuterXml);
		}

		private static Dictionary<string, string> Users = new Dictionary<string, string>();

		private static void AddUser(MetaData md)
		{
			string user = md.GetValueString("user");
			string id = md.GetValueString("id");
			AddUser(user, id);
		}
		private static void AddUser(string user, string id)
		{	
			if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(id))
			{
				if (!Users.ContainsKey(id)) {
					Users.Add(id, user);					
				}

				Users[id] = user;
			}
		}

		private static string FindId(string user)
		{	
			return Users.FirstOrDefault( it => it.Value == user ).Key;
		}

		private static string DefaultPrompt {get; set;}

		private static string Prompt(string prompt)
		{			
			Console.Write($"{prompt}");
			return Console.ReadLine();
		}

		private static void Print(string message)
		{			
			Console.Write($"{Environment.NewLine}{message}{Environment.NewLine}{DefaultPrompt}");
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
			DefaultPrompt = $"[{userName}]: ";
			MetaData mtdt = new MetaData();
			source.Connect().GetAwaiter().GetResult();
			
			SetId(mtdt, source.ConnectionId);
			SetUser(mtdt, userName);
			AddUser(userName, source.ConnectionId);
			Console.WriteLine($"Welcome: {userName}!");

			source.Handle.OnDisconnected( id =>
			{
				Users.Remove(id);
			});

			source.Handle.String("Chat", (md, data) =>
			{
				AddUser(md);
				string user = md.GetValueString("user");
				Print($"{user}: {data}");
			});

			source.Handle.String("ChatTo", (md, data) =>
			{
				AddUser(md);
				string user = md.GetValueString("user");
				Print($"{user}: {data}");
			});

			source.Handle.Binary("File", (md, data) =>
			{
				AddUser(md);
				string user = md.GetValueString("user");
				string fileName = md.GetValueString("fileName");				
				Print($"{user}: {fileName} length {data.Length}");
			});

			source.Handle.Xml("Xml", (md, data) =>
			{
				AddUser(md);
				string user = md.GetValueString("user");
				string content = md.GetValueString("content");
				Print($"{user}: Xml {Environment.NewLine} {content} {Environment.NewLine}");
			});

			string message = "Connected";
			string path = string.Empty;
			string to = string.Empty;
			bool exit = false;

			do
			{
				message = Prompt($"{Environment.NewLine}{DefaultPrompt}");

				switch (message)
				{
					case "users":
						foreach(var it in Users)
						{
							Console.WriteLine($"{it.Key}: {it.Value}");
						}
					break;
					case "clear":
						Console.Clear();
						break;
					case "quit":
						source.Raise.String("Chat", "Disconnected", mtdt);
						return;
					case "file":
						path = Prompt("Enter path: ");
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							SetFileInfo(mtdt, path);
							source.Raise.Binary("File", bytes, mtdt);
						}
						break;
					case "xml":
						path = Prompt("Enter path: ");						
						if (File.Exists(path))
						{
							XmlDocument doc = new XmlDocument();
							string contents = File.ReadAllText(path);
							doc.LoadXml(contents);
							SetXmlData(mtdt, doc);
							source.Raise.Xml("Xml", doc, mtdt);
						}
						break;
					case "to":
						string user = Prompt("Send private message to: ");
						to = FindId(user);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = Prompt($"Private message for {user}: ");
							source.Raise.StringTo("ChatTo", to, message, mtdt);
						}else
						{
							Console.WriteLine($"User not found: {user}");
						}
					break;
					default:
						source.Raise.String("Chat", message, mtdt);
						break;
				}				
			} 
			while (!exit);
		}
	}
}