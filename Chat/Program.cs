using System;
using Communicator;
using System.IO;
using System.Collections.Generic;
using Communicator.Core;

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
			IEventSource source = EventSourceFactory.Get(url, new JsonSerializer());
			
			List<MetaData> mtdt = new List<MetaData>();
			source.Connect().GetAwaiter().GetResult();
			
			mtdt.SetId(source.ConnectionId);
			mtdt.SetUser(userName);
			Connections.AddUser(userName, source.ConnectionId);
			Console.WriteLine($"Welcome: {userName}!");

			
			var onConnected = source.Observables.GetOnConnected(); 
			onConnected.Subscribe(id => Connections.Remove(id));
			
 			var onChat = source.Observables.GetString<List<MetaData>>("Chat");
			onChat.Subscribe((msg) =>
			{
				var md = msg.MetaData;
				Connections.AddUser(md);
				string user = md.Get("user");
				$"{user}: {msg.Data}".Print();
			});

			var onFile = source.Observables.GetBinary<List<MetaData>>("File");
			onFile.Subscribe((msg) =>
			{
				var md = msg.MetaData;
				Connections.AddUser(md);
				string user = md.Get("user");
				string fileName = md.Get("fileName");				
				$"{user}: {fileName} length {msg.Data.Length}".Print();
			});

			var onPerson = source.Observables.GetSerialized<Person, List<MetaData>>("Person");
			onPerson.Subscribe((msg) =>
			{
				var md = msg.MetaData;
				Connections.AddUser(md);

				string user = md.Get("user");				
				$"{user}: Person {{ Name = {msg.Data.Name}, Age = {msg.Data.Age} }}".Print();
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
						source.Send.String("Chat", new StringMessage<List<MetaData>>("Disconnected", mtdt));
						return;
					case "file":
						path = "Enter path: ".Prompt();
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							mtdt.SetFileInfo(path);
							source.Send.Binary("File", new BinaryMessage<List<MetaData>>(bytes, mtdt));							
						}
						break;					
					case "to":
						string user = "Send private message to: ".Prompt();
						to = Connections.FindId(user);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = $"Private message for {user}: ".Prompt();
							source.Send.String("Chat", new StringMessage<List<MetaData>>(message, mtdt), new string[] { to });
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
						source.Send.Serialized("Person", new StringSerializedMessage<Person, List<MetaData>>(p, mtdt));
					break;
					default:						
						source.Send.String("Chat", new StringMessage<List<MetaData>>(message, mtdt));
						break;
				}				
			} 
			while (!exit);
		}
	}
}