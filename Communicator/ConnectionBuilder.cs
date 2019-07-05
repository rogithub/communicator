using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Communicator
{

    internal class ConnectionBuilder 
    {        
        internal static HubConnection Build(string urlServer)
        {
            var connection = new HubConnectionBuilder()
				.WithUrl(urlServer)
				.Build();

            connection.StartAsync();
            
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await connection.StartAsync();
            };

            return connection;
        }
    }
}