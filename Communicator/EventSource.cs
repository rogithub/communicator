using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Communicator.Core;
using System;

namespace Communicator
{
    public interface IEventSource 
    {
        IObservableFactory Observables { get; }
        IEventSender Send { get; }
        string ConnectionId { get; }
        Task<Guid> Connect<T>(T metaData) where T : new();
    }

    public static class EventSourceFactory
    {
        public static IEventSource Get(string urlServer, IDataSerializer dataSerializer)
        {
            return new EventSource(urlServer, dataSerializer);
        }
    }


    internal class EventSource : IEventSource
    {
        private HubConnection Connection { get; set; }
        public IObservableFactory Observables {get; private set;}
        public IEventSender Send {get; private set;}
        public IDataSerializer DataSerializer {get; private set;}

        public string ConnectionId {get; private set;}

        public EventSource(string urlServer, IDataSerializer serializer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.Observables = new ObservableFactory(this.Connection, serializer);
            this.Send = new EventSender(this.Connection, serializer);
        }

        public async Task<Guid> Connect<T>(T metaData) where T: new()
        {        
            await Connection.StartAsync();
            ConnectionId = await Connection.InvokeAsync<string>(EventNames.GetConnectionId);            
            Guid taskId = await Send.String(new EventInfo(EventNames.OnConnected), new StringMessage<T>(ConnectionId, metaData));

            return taskId;
        }
    }
}