using System;
using Communicator;
using System.IO;
using System.Xml;

namespace Chat
{
    class Program
	{		

		private static ActiveConnections Connections { get; set; }
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				StringHelper.PrintHelp();
				return;
			}

			Connections = new ActiveConnections();
			string userName = args[0];
			

			string url = "http://localhost:5000/communicator";
			EventSource source = new EventSource(url);			
			
			MetaData mtdt = new MetaData();
			source.Connect().GetAwaiter().GetResult();
			
			mtdt.SetId(source.ConnectionId);
			mtdt.SetUser(userName);
			Connections.AddUser(userName, source.ConnectionId);
			Console.WriteLine($"Welcome: {userName}!");

			source.Handle.OnDisconnected( id =>
			{
				Connections.Remove(id);
			});

			source.Handle.String("Chat", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.GetValueString("user");
				$"{user}: {data}".Print();
			});

			source.Handle.String("ChatTo", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.GetValueString("user");
				$"{user}: {data}".Print();
			});

			source.Handle.Binary("File", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.GetValueString("user");
				string fileName = md.GetValueString("fileName");				
				$"{user}: {fileName} length {data.Length}".Print();
			});

			source.Handle.Xml("Xml", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.GetValueString("user");
				string content = md.GetValueString("content");
				$"{user}: Xml {Environment.NewLine} {content} {Environment.NewLine}".Print();
			});

			source.Handle.Json<Person>("person", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.GetValueString("user");				
				$"{user}: Person {{ Name = {data.Name}, Age = {data.Age} }}".Print();
			});

			string message = "Connected";
			string path = string.Empty;
			string to = string.Empty;
			bool exit = false;

			do
			{
				message = $"{Environment.NewLine}{StringHelper.DefaultPrompt}".Prompt();

				switch (message)
				{
					case "users":
						Connections.Print();
					break;
					case "clear":
						Console.Clear();
						break;
					case "quit":
						source.Raise.String("Chat", "Disconnected", mtdt);
						return;
					case "file":
						path = "Enter path: ".Prompt();
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							mtdt.SetFileInfo(path);
							source.Raise.Binary("File", bytes, mtdt);
						}
						break;
					case "xml":
						path = "Enter path: ".Prompt();						
						if (File.Exists(path))
						{
							XmlDocument doc = new XmlDocument();
							string contents = File.ReadAllText(path);
							doc.LoadXml(contents);							
							mtdt.SetXmlData(doc);
							source.Raise.Xml("Xml", doc, mtdt);
						}
						break;
					case "to":
						string user = "Send private message to: ".Prompt();
						to = Connections.FindId(user);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = $"Private message for {user}: ".Prompt();
							source.Raise.StringTo("ChatTo", to, message, mtdt);
						}else
						{
							Console.WriteLine($"User not found: {user}");
						}
					break;
					case "person":
						string name = "Person Name: ".Prompt();
						string ageStr = "Person Age: ".Prompt();
						int age = 0;
						int.TryParse(ageStr.Trim(), out age);
						Person p = new Person() { Name = name, Age = age };
						source.Raise.SendJson("Person", p, mtdt);
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