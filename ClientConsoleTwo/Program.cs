using System;
using Communicator;
using System.Threading;
using System.Xml;

namespace ClientConsoleTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:5000/communicator";
            Server server = new Server(url);
			
			CancellationTokenSource ts = new CancellationTokenSource(); 			

			server.HandlerFactory.AddHandler("ClientConsole", (string user, string data) =>
			{
				Console.WriteLine($"user {user} sent string: {data}");
			});

			XmlDocument doc = new XmlDocument();
			doc.LoadXml($"<Response>Got Response</Response>");

			var task = server.EventFactory.RaiseEvent("ClientConsoleTwo", "Jhon", doc, ts.Token);
		
			Guid id = task.GetAwaiter().GetResult();
			
			Console.Read();
        }
    }
}
