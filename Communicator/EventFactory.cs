using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;

namespace Communicator
{
    public interface IEventFactory<TMetaData>
    {
        Task<Guid> Binary(string eventName, byte[] data, TMetaData meta);
        Task<Guid> String(string eventName, string data, TMetaData meta);
        Task<Guid> Json<T>(string eventName, T data, TMetaData meta);
        Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer);
        Task<Guid> StringTo(string eventName, string connectionId, string data, TMetaData meta);
    }
    internal class EventFactory<TMetaData> : IEventFactory<TMetaData>
    {
        private HubConnection Connection { get; set; }
        private IStringSerializer DefaultSerializer { get; set; }
        internal EventFactory(HubConnection connection, IStringSerializer serializer)
        {
            this.Connection = connection;  
            this.DefaultSerializer = serializer;
        }
           
        public Task<Guid> Json<T>(string eventName, T data, TMetaData meta)
        {
            return Serialized(eventName, data, meta, (d) => DefaultSerializer.Serialize(d));
        }

        public Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer)
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, DefaultSerializer.Serialize(meta), serializer(data));
        }

        public Task<Guid> Binary(string eventName, byte[] data, TMetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinary, eventName, DefaultSerializer.Serialize(meta), data);
        }
        
        public Task<Guid> String(string eventName, string data, TMetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, DefaultSerializer.Serialize(meta), data);
        }

        // TODO: 
        // * Remove EventNames.SendString
        // * Always use EventNames.SendStringTo sending an array of connection Ids, emtpy means to all.
        // * Do the same for binary
        // * Implement Rx version of it using IMessage.cs file
        public Task<Guid> StringTo(string eventName, string connectionId, string data, TMetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, connectionId, eventName, DefaultSerializer.Serialize(meta), data);
        }
    }
}
