using System;
using Communicator;
using System.IO;
using System.Collections.Generic;
using Communicator.Core;
using System.Threading.Tasks;

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
			List<KeyValue> mtdt = new List<KeyValue>();
			mtdt.Set("user", userName);

			source.Connect(mtdt).ContinueWith( t => { 
				Console.WriteLine("{0}", url);
				Console.WriteLine("Error when trying to connect");
				Console.WriteLine("{0}: {1}",
				t.Exception.InnerException.GetType().Name,
				t.Exception.InnerException.Message);
				
				Console.WriteLine();

			}, TaskContinuationOptions.OnlyOnFaulted).GetAwaiter().GetResult();
	
			mtdt.Set("id", source.ConnectionId);
			Connections.AddUser(mtdt);
			Console.WriteLine($"Welcome: {userName}!");
			Action help = () => Console.WriteLine("keywords [ help, users, clear, file, to, person, quit ]");
			help();

			var onDisconnected = source.Observables.GetOnDisconnected();
			var onConnected = source.Observables.GetOnConnected();
			var onChat = source.Observables.GetString("Chat");
			var onFile = source.Observables.GetBinary("File");
			var onPerson = source.Observables.GetSerialized<Person>("Person");

			onConnected.Subscribe(m => Connections.AddUser(m.MetaData));
			onChat.Subscribe(m => Connections.AddUser(m.MetaData));
			onFile.Subscribe(m => Connections.AddUser(m.MetaData));
			onPerson.Subscribe(m => Connections.AddUser(m.MetaData));

			onConnected.Subscribe(msg => {
				string user = msg.MetaData.Get("user");
				Console.WriteLine($" User {user} joined!");
			});
			
			
			onDisconnected.Subscribe(id => Connections.Remove(id));
						
			onChat.Subscribe((msg) =>
			{
				var md = msg.MetaData;				
				string user = md.Get("user");
				$"{user}: {msg.Data}".Print();
			});

			onFile.Subscribe((msg) =>
			{
				var md = msg.MetaData;
				string user = md.Get("user");
				string fileName = md.Get("fileName");				
				$"{user}: {fileName} length {msg.Data.Length}".Print();
			});

			onPerson.Subscribe((msg) =>
			{
				var md = msg.MetaData;
				string user = md.Get("user");				
				$"{user}: Person {{ Name = {msg.Data.Name}, Age = {msg.Data.Age} }}".Print();
			});
			

			string message = "Connected";
			string path = string.Empty;
			string to = string.Empty;
			bool exit = false;

			do
			{
				message = $"{Environment.NewLine}{StringHelper.DefaultPrompt}".Prompt().Trim();

				switch (message)
				{
					case "help":
						help();
					break;
					case "users":
						Connections.Print();
					break;
					case "clear":
						Console.Clear();
						break;
					case "quit":
						source.Send.String(new EventInfo("Chat"), new StringMessage("Disconnected", mtdt));
						return;
					case "file":
						path = "Enter path: ".Prompt();
						if (File.Exists(path))
						{
							byte[] bytes = System.IO.File.ReadAllBytes(path);
							mtdt.SetFileInfo(path);
							source.Send.Binary(new EventInfo("File"), new BinaryMessage(bytes, mtdt));							
						}
						break;					
					case "to":
						string user = "Send private message to: ".Prompt();
						to = Connections.FindId(user);
						if (!string.IsNullOrWhiteSpace(to))
						{
							message = $"Private message for {user}: ".Prompt();
							source.Send.String(new EventInfo("Chat", to, null), new StringMessage(message, mtdt));
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
						source.Send.Serialized(new EventInfo("Person"), new StringSerializedMessage<Person>(p, mtdt));
					break;
					default:						
						source.Send.String(new EventInfo("Chat"), new StringMessage(message, mtdt));
						break;
				}				
			} 
			while (!exit);
        }
    }
}
