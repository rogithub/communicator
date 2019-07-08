using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

namespace Communicator
{
    public class EventSource 
    {
        private HubConnection Connection { get; set; }
        public IHandlerFactory Handle {get; private set;}
        public IEventFactory Send {get; private set;}

        public string ConnectionId {get; private set;}

        public EventSource(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.Handle = new HandlerFactory(this.Connection);
            this.Send = new EventFactory(this.Connection);            
        }

        public async Task<string> Connect()
        {            
            //Connection.Closed += async (error) =>
            //{
            //    await Task.Delay(new Random().Next(0,5) * 1000);
            //    await Connection.StartAsync();
            //};

            await Connection.StartAsync();
            ConnectionId = await Connection.InvokeAsync<string>(EventNames.GetConnectionId);
            
            return ConnectionId;
        }
    }
}