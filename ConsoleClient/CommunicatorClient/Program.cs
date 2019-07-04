
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace CommunicatorClient
{
	class Program
	{
		static void Main(string[] args)
		{
			//Set connection
			var connection = new HubConnectionBuilder()
				.WithUrl("http://localhost:53505/communicator")
				.Build();


			connection.StartAsync().Wait();

			connection.InvokeAsync<string>("SendMessage", "Rodrigo", "Hola Mundo").ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					Console.WriteLine("There was an error calling send: {0}",
									  task.Exception.GetBaseException());
				}
				else
				{
					Console.WriteLine(task.Result);
				}
			}).GetAwaiter().GetResult();

			Console.Read();

		}
	}
}
