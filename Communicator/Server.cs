using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public class Server 
    {
        private HubConnection Connection { get; set; }
        public IHandlerFactory HandlerFactory {get; private set;}
        public IEventFactory EventFactory {get; private set;}
        public Server(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.HandlerFactory = new HandlerFactory(this.Connection);
            this.EventFactory = new EventFactory(this.Connection);
        }
    }
}