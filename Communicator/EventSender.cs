using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using Communicator.Rx;

namespace Communicator
{
    public interface IEventSender
    {
        Task<Guid> Binary<M>(string eventName, BinaryMessage<M> message) where M : new();
        Task<Guid> Serialized<D, M>(string eventName, StringSerializedMessage<D, M> message) where D : new() where M : new();
        Task<Guid> String<M>(string eventName, StringMessage<M> message) where M : new();

        Task<Guid> Binary<M>(string eventName, BinaryMessage<M> message, IEnumerable<string> to) where M : new();
        Task<Guid> Serialized<D, M>(string eventName, StringSerializedMessage<D, M> message, IEnumerable<string> to) where D : new() where M : new();
        Task<Guid> String<M>(string eventName, StringMessage<M> message, IEnumerable<string> to) where M : new();
    }
    internal class EventSender : IEventSender
    {
        private HubConnection Connection { get; set; }
        private IStringSerializer DefaultSerializer { get; set; }
        internal EventSender(HubConnection connection, IStringSerializer serializer)
        {
            this.Connection = connection;  
            this.DefaultSerializer = serializer;
        }                   

        public Task<Guid> Serialized<D, M>(string eventName, StringSerializedMessage<D, M> message, IEnumerable<string> to) where D : new() where M : new()
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), DefaultSerializer.Serialize(message.Data));
        }
        public Task<Guid> Serialized<D, M>(string eventName, StringSerializedMessage<D, M> message)  where D : new() where M : new()
        {
            return Serialized(eventName, message, Array.Empty<string>());
        }
        
        public Task<Guid> String<M>(string eventName, StringMessage<M> message, IEnumerable<string> to) where M : new()
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }
        public Task<Guid> String<M>(string eventName, StringMessage<M> message)  where M : new()
        {            
            return String(eventName, message, Array.Empty<string>());
        }
        

        public Task<Guid> Binary<M>(string eventName, BinaryMessage<M> message, IEnumerable<string> to) where M : new()
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinaryTo, to, eventName, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }        
        public Task<Guid> Binary<M>(string eventName, BinaryMessage<M> message)  where M : new()
        {            
            return Binary(eventName, message, Array.Empty<string>());
        }
    }
}
