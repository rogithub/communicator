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

			server.HandlerFactory.AddHandler("OnRequestXml", (string user, string data) =>
			{
				Console.WriteLine($"user ${user} requested an xml.");
			});

			XmlDocument doc = new XmlDocument();
			doc.LoadXml($"<Response>Got Response</Response>");

			var task = server.EventFactory.RaiseEvent("OnResponseXml", "Juan", doc, ts.Token);
		
			Guid id = task.GetAwaiter().GetResult();
			Console.WriteLine($"id: {id}");
		
			
			Console.Read();
        }
    }
}
