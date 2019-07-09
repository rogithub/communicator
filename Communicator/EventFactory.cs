using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using Communicator.Rx;

namespace Communicator
{
    public interface IEventFactory<M>  where M : new()
    {
        Task<Guid> Binary(string eventName, BinaryMessage<M> message);
        Task<Guid> Serialized<T>(string eventName, StringSerializedMessage<T, M> message) where T : new();
        Task<Guid> String(string eventName, StringMessage<M> message);

        Task<Guid> Binary(string eventName, BinaryMessage<M> message, IEnumerable<string> to);
        Task<Guid> Serialized<T>(string eventName, StringSerializedMessage<T, M> message, IEnumerable<string> to) where T : new();
        Task<Guid> String(string eventName, StringMessage<M> message, IEnumerable<string> to);
    }
    internal class EventFactory<M> : IEventFactory<M> where M : new()
    {
        private HubConnection Connection { get; set; }
        private IStringSerializer DefaultSerializer { get; set; }
        internal EventFactory(HubConnection connection, IStringSerializer serializer)
        {
            this.Connection = connection;  
            this.DefaultSerializer = serializer;
        }                   

        public Task<Guid> Serialized<T>(string eventName, StringSerializedMessage<T, M> message, IEnumerable<string> to) where T : new()
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), DefaultSerializer.Serialize(message.Data));
        }

        public Task<Guid> Binary(string eventName, BinaryMessage<M> message, IEnumerable<string> to) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinaryTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }            

        public Task<Guid> String(string eventName, StringMessage<M> message, IEnumerable<string> to) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }

        public Task<Guid> Serialized<T>(string eventName, StringSerializedMessage<T, M> message) where T : new()
        {
            return Serialized(eventName, message, Array.Empty<string>());
        }

        public Task<Guid> Binary(string eventName, BinaryMessage<M> message) 
        {            
            return Binary(eventName, message);
        }            

        public Task<Guid> String(string eventName, StringMessage<M> message) 
        {            
            return String(eventName, message, Array.Empty<string>());
        }
    }
}
