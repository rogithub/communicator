using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;

namespace Communicator
{
    public interface IEventFactory<TMetaData>
    {
        Task<Guid> Binary(string eventName, byte[] data, TMetaData meta);
        Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer);
        Task<Guid> String(string eventName, string data, TMetaData meta);

        Task<Guid> Binary(string eventName, byte[] data, TMetaData meta, IEnumerable<string> to);
        Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer, IEnumerable<string> to);
        Task<Guid> String(string eventName, string data, TMetaData meta, IEnumerable<string> to);
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

        public Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer, IEnumerable<string> to)
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(meta), serializer(data));
        }

        public Task<Guid> Binary(string eventName, byte[] data, TMetaData meta, IEnumerable<string> to) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinaryTo, to, eventName, DefaultSerializer.Serialize(meta), data);
        }            

        public Task<Guid> String(string eventName, string data, TMetaData meta, IEnumerable<string> to) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(meta), data);
        }

        public Task<Guid> Serialized<T>(string eventName, T data, TMetaData meta, Func<T, string> serializer)
        {
            return Serialized(eventName, data, meta, serializer, Array.Empty<string>());
        }

        public Task<Guid> Binary(string eventName, byte[] data, TMetaData meta) 
        {            
            return Binary(eventName, data, meta, Array.Empty<string>());
        }            

        public Task<Guid> String(string eventName, string data, TMetaData meta) 
        {            
            return String(eventName, data, meta, Array.Empty<string>());
        }
    }
}
