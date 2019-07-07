using System.Xml;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{
    public interface IEventFactory
    {
        Task<Guid> Xml(string eventName, XmlDocument data, MetaData meta);
        Task<Guid> Binary(string eventName, byte[] data, MetaData meta);
        Task<Guid> String(string eventName, string data, MetaData meta);
        Task<Guid> SendJson<T>(string eventName, T data, MetaData meta);
        Task<Guid> StringTo(string eventName, string connectionId, string data, MetaData meta);
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
            return Connection.InvokeAsync<Guid>(EventNames.SendXml, eventName, SerializationJson.Serialize(meta), data);
        }

        public Task<Guid> SendJson<T>(string eventName, T data, MetaData meta)
        {
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, SerializationJson.Serialize(meta), SerializationJson.Serialize(data));
        }

        public Task<Guid> Binary(string eventName, byte[] data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendBinary, eventName, SerializationJson.Serialize(meta), data);
        }
        
        public Task<Guid> String(string eventName, string data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendString, eventName, SerializationJson.Serialize(meta), data);
        }

        public Task<Guid> StringTo(string eventName, string connectionId, string data, MetaData meta) 
        {            
            return Connection.InvokeAsync<Guid>(EventNames.SendStringTo, connectionId, eventName, SerializationJson.Serialize(meta), data);
        }
    }
}
