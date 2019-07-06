using Microsoft.AspNetCore.SignalR.Client;


namespace Communicator
{

    internal class ConnectionBuilder 
    {        
        internal static HubConnection Build(string urlServer)
        {
            var connection = new HubConnectionBuilder()
				.WithUrl(urlServer)
				.Build(); 

            return connection;
        }
    }
}