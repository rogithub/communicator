using System;
using Communicator;
using System.Threading;
using System.Xml;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
			string url = "http://localhost:5000/communicator";
            Server server = new Server(url);
			CancellationTokenSource ts = new CancellationTokenSource(); 
			
			server.HandlerFactory.AddHandler("Chat", (string user, string data) =>
			{
				Console.WriteLine($"[{user}]: {data}");
			});

			string message = "Connected";
			while(message != "quit") {				
				server.EventFactory.RaiseEvent("Chat", "Console", message, ts.Token);
				message = Console.ReadLine();
			}
					
			server.EventFactory.RaiseEvent("Chat", "Console", "Disconnected", ts.Token);
			
        }
    }
}
