using System;
using Communicator;
using System.IO;

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
				string user = md.Get("user");
				$"{user}: {data}".Print();
			});

			source.Handle.String("ChatTo", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.Get("user");
				$"{user}: {data}".Print();
			});

			source.Handle.Binary("File", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.Get("user");
				string fileName = md.Get("fileName");				
				$"{user}: {fileName} length {data.Length}".Print();
			});			

			source.Handle.Json<Person>("Person", (md, data) =>
			{
				Connections.AddUser(md);
				string user = md.Get("user");				
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
						source.Send.String("Chat", "Disconnected", mtdt);
						return;
					case "file":
						path = "Enter path: ".Prompt();
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							mtdt.SetFileInfo(path);
							source.Send.Binary("File", bytes, mtdt);
						}
						break;					
					case "to":
						string user = "Send private message to: ".Prompt();
						to = Connections.FindId(user);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = $"Private message for {user}: ".Prompt();
							source.Send.StringTo("ChatTo", to, message, mtdt);
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
						source.Send.Json("Person", p, mtdt);
					break;
					default:
						source.Send.String("Chat", message, mtdt);
						break;
				}				
			} 
			while (!exit);
		}
	}
}