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
			
			server.HandlerFactory.AddHandler("ClientConsoleTwo", (string user, XmlDocument data) =>
			{
				Console.WriteLine($"{user} sent xml: \n\r {data.OuterXml}");
			});

			CancellationTokenSource ts = new CancellationTokenSource(); 
			var task = server.EventFactory.RaiseEvent("ClientConsole", "Rodrigo", "Hello World!", ts.Token);
            
			Guid id = task.GetAwaiter().GetResult();
			Console.WriteLine($"id: {id}");
			
			Console.Read();
        }
    }
}
