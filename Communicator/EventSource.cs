using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Communicator.Core;
using System;

namespace Communicator
{
    public interface IEventSource 
    {
        IObservableFactory GetObservablesFactory(IStringDeserializer deserializer);
        IEventSender GetEventSender(IStringSerializer serializer);
        Task<string> GetConnectionId();
        Task Connect();
    }

    public static class EventSourceFactory
    {
        public static IEventSource Get(string urlServer)
        {
            return new EventSource(urlServer);
        }
    }


    internal class EventSource : IEventSource
    {
        private HubConnection Connection { get; set; }        

        public EventSource(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);            
        }

        public Task Connect()
        {
            return Connection.StartAsync();
        }        

        public IObservableFactory GetObservablesFactory(IStringDeserializer deserializer)
        {
            return new ObservableFactory(Connection, deserializer);
        }

        public IEventSender GetEventSender(IStringSerializer serializer)
        {
            return new EventSender(Connection, serializer);
        }

        public Task<string> GetConnectionId()
        {
            return Connection.InvokeAsync<string>(EventNames.GetConnectionId); 
        }
    }
}