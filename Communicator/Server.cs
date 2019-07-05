using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public class Server 
    {
        private HubConnection Connection { get; set; }
        public IHandlerFactory Handle {get; private set;}
        public IEventFactory Raise {get; private set;}
        public Server(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.Handle = new HandlerFactory(this.Connection);
            this.Raise = new EventFactory(this.Connection);
        }
    }
}