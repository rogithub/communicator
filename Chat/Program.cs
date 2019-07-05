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
			Server server = new Server(url);

			server.HandlerFactory.AddHandler("Chat", (string user, string data) =>
			{
				Console.WriteLine($"[{user}]: {data}");
			});

			server.HandlerFactory.AddHandler("File", (string user, byte[] data) =>
			{
				Console.WriteLine($"[{user}]: file length {data.Length}");
			});

			server.HandlerFactory.AddHandler("Xml", (string user, XmlDocument data) =>
			{
				Console.WriteLine($"[{user}]: Xml \n\r {data.OuterXml}");
			});

			string message = "Connected";
			string path = string.Empty;
			bool exit = false;

			while (!exit)
			{
				message = Console.ReadLine();

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
							server.EventFactory.RaiseEvent("File", userName, bytes);
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
							server.EventFactory.RaiseEvent("Xml", userName, doc);
						}
						break;
					default:
						server.EventFactory.RaiseEvent("Chat", userName, message);
						break;
				}
			}

			server.EventFactory.RaiseEvent("Chat", userName, "Disconnected");
		}
	}
}