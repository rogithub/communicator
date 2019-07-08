using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{
    public interface IEventFactory
    {
        Task<Guid> Binary(string eventName, byte[] data, MetaData meta);
        Task<Guid> String(string eventName, string data, MetaData meta);
        Task<Guid> Json<T>(string eventName, T data, MetaData meta);
        Task<Guid> Serialized<T>(string eventName, T data, MetaData meta, Func<T, string> serializer);
        Task<Guid> StringTo(string eventName, string connectionId, string data, MetaData meta);
    }
    internal class EventFactory : IEventFactory
    {
        private HubConnection Connection { get; set; }
        private IStringSerializer JsonSerializer { get; set; }
        internal EventFactory(HubConnection connection)
        {
            this.Connection = connection;  
            this.JsonSerializer = new DefaultJsonSerializer();          
        }
           
        public Task<Guid> Json<T>(string eventName, T data, MetaData meta)
        {
            return Serialized(eventName, data, meta, (d) => JsonSerializer.Serialize(d));
        }

        public Task<Guid> Serialized<T>(string eventName, T data, MetaData meta, Func<T, string> serializer)
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, JsonSerializer.Serialize(meta), serializer(data));
        }

        public Task<Guid> Binary(string eventName, byte[] data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinary, eventName, JsonSerializer.Serialize(meta), data);
        }
        
        public Task<Guid> String(string eventName, string data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, JsonSerializer.Serialize(meta), data);
        }

        public Task<Guid> StringTo(string eventName, string connectionId, string data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, connectionId, eventName, JsonSerializer.Serialize(meta), data);
        }
    }
}
