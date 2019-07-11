using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using System.Collections.Generic;

namespace Communicator
{
    public interface IEventSender
    {        
        Task<Guid> Binary<M>(EventInfo info, BinaryMessage<M> message) where M : new();
        Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message) where D : new() where M : new();
        Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message, IStringSerializer dataSerializer , IStringSerializer metaSerializer) where D : new() where M : new();
        Task<Guid> String<M>(EventInfo info, StringMessage<M> message) where M : new();        

        Task<Guid> Binary(EventInfo info, byte[] message);
        Task<Guid> Serialized<T>(EventInfo info, T message) where T : new();
        Task<Guid> String(EventInfo info, string message);

        Task<Guid> Binary(EventInfo info, BinaryMessage message);
        Task<Guid> Serialized<T>(EventInfo info, StringSerializedMessage<T> message) where T : new();
        Task<Guid> Serialized<T>(EventInfo info, StringSerializedMessage<T> message, IStringSerializer dataSerializer , IStringSerializer metaSerializer) where T : new();
        Task<Guid> String(EventInfo info, StringMessage message);
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

        public Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message) where D : new() where M : new()
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, info.EventName, info.To, DefaultSerializer.Serialize(message.MetaData), DefaultSerializer.Serialize(message.Data));
        }
        
        public Task<Guid> String<M>(EventInfo info, StringMessage<M> message) where M : new()
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, info.EventName, info.To, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }        

        public Task<Guid> Binary<M>(EventInfo info, BinaryMessage<M> message) where M : new()
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinaryTo, info.EventName, info.To, DefaultSerializer.Serialize(message.MetaData), message.Data);
        }        

        public Task<Guid> Binary(EventInfo info, byte[] data)
        {
            var message = new NoMetaDataBinaryMessage(data);
            return Binary(info, message);            
        }

        public Task<Guid> Serialized<T>(EventInfo info, T data) where T : new()
        {
            var message = new NoMetaDataSerializedMessage<T>(data);
            return Serialized(info, message);            
        }

        public Task<Guid> String(EventInfo info, string data)
        {
            var message = new NoMetaDataStringMessage(data);
            return String(info, message);
        }

        public Task<Guid> Binary(EventInfo info, BinaryMessage message)
        {
            return Binary<List<MetaData>>(info, message);
        }

        public Task<Guid> Serialized<T>(EventInfo info, StringSerializedMessage<T> message) where T : new()
        {
            return Serialized<T, List<MetaData>>(info, message);
        }

        public Task<Guid> String(EventInfo info, StringMessage message)
        {
            return String<List<MetaData>>(info, message);
        }

        public Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message, IStringSerializer dataSerializer , IStringSerializer metaSerializer)
            where D : new()
            where M : new()
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, info.EventName, info.To, metaSerializer.Serialize(message.MetaData), dataSerializer.Serialize(message.Data));
        }

        public Task<Guid> Serialized<T>(EventInfo info, StringSerializedMessage<T> message, IStringSerializer dataSerializer , IStringSerializer metaSerializer) where T : new()
        {
            return Serialized<T, List<MetaData>>(info, message, dataSerializer, metaSerializer);
        }
    }
}
