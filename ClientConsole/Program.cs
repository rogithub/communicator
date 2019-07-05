using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set connection
			var connection = new HubConnectionBuilder()
				.WithUrl("http://localhost:5000/communicator")
				.Build();


			connection.StartAsync();

			connection.InvokeAsync<string>("SendMessage", "Rodrigo Client", "Hola ").ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					Console.WriteLine("There was an error calling send: {0}",
									  task.Exception.GetBaseException());
				}
				else
				{
					
					Console.WriteLine("Message Sent");
					Console.WriteLine(task.Result);
				}
			});

			connection.On<string, string>("ReceiveMessage", (user, message) => {
				var newMessage = $"{user}: {message}";
				Console.WriteLine(newMessage);
			});

            Console.Read();
        }
    }
}
