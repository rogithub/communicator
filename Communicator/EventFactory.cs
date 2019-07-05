using System.Xml;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace Communicator
{
    public interface IEventFactory
    {
        Task<Guid> Xml(string eventName, XmlDocument data, MetaData meta);
        Task<Guid> Binary(string eventName, byte[] data, MetaData meta);
        Task<Guid> String(string eventName, string data, MetaData meta);
    }
    internal class EventFactory : IEventFactory
    {
        private HubConnection Connection { get; set; }
        internal EventFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public Task<Guid> Xml(string eventName, XmlDocument data, MetaData meta)
        {
            return Connection.InvokeAsync<Guid>("SendXml", eventName, SerializationXml.Serialize(meta), data);
        }

        public Task<Guid> Binary(string eventName, byte[] data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>("SendBinary", eventName, SerializationXml.Serialize(meta), data);
        }
        
        public Task<Guid> String(string eventName, string data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>("SendString", eventName, SerializationXml.Serialize(meta), data);
        }
    }
}
