using Microsoft.AspNetCore.SignalR.Client;


namespace Communicator
{

    internal class ConnectionBuilder 
    {        
        internal static IHubConnection Build(string urlServer)
        {
            var connection = new HubConnectionBuilder()
				.WithUrl(urlServer)
				.Build(); 

            return new HubConnectionProxy(connection);
        }
    }
}