using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

namespace Communicator
{
    public class EventSource 
    {
        private HubConnection Connection { get; set; }
        public IHandlerFactory Handle {get; private set;}
        public IEventFactory Raise {get; private set;}

        public string ConnectionId {get; private set;}

        public EventSource(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.Handle = new HandlerFactory(this.Connection);
            this.Raise = new EventFactory(this.Connection);            
        }

        public async Task<Guid> Connect(MetaData md)
        {
            Guid eventId;
            
            Action<Task> setId = async (t) => {
                string id = await Connection.InvokeAsync<string>(EventNames.GetConnectionId); 
                this.ConnectionId = id;                
                eventId = await this.Raise.String(EventNames.OnConnected, id, md);
            };

            await Connection.StartAsync().ContinueWith(setId);

            Connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await Connection.StartAsync().ContinueWith(setId);
            };

            return eventId;
        }
    }
}