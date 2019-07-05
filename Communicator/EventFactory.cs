using System.Xml;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace Communicator
{
    public interface IEventFactory
    {
        Task<Guid> Xml(string eventName, string user, XmlDocument doc);
        Task<Guid> Binary(string eventName, string user, byte[] data) ;        
        Task<Guid> String(string eventName, string user, string data) ;
    }
    internal class EventFactory : IEventFactory
    {
        private HubConnection Connection { get; set; }
        internal EventFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public Task<Guid> Xml(string eventName, string user, XmlDocument doc)
        {            
            return Connection.InvokeAsync<Guid>("SendXml", eventName, user, doc);
        }

        public Task<Guid> Binary(string eventName, string user, byte[] data) 
        {            
            return Connection.InvokeAsync<Guid>("SendBinary", eventName, user, data);
        }
        
        public Task<Guid> String(string eventName, string user, string data) 
        {            
            return Connection.InvokeAsync<Guid>("SendString", eventName, user, data);
        }
    }
}