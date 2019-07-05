using System.Xml;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace Communicator
{
    public interface IEventFactory
    {
        Task<Guid> RaiseEvent(string eventName, string user, XmlDocument doc, CancellationToken token);
        Task<Guid> RaiseEvent<T>(string eventName, string user, T json, CancellationToken token)  where  T : new () ;
        Task<Guid> RaiseEvent(string eventName, string user, byte[] data, CancellationToken token) ;
        
        Task<Guid> RaiseEvent(string eventName, string user, string data, CancellationToken token) ;
    }
    internal class EventFactory : IEventFactory
    {
        private HubConnection Connection { get; set; }
        internal EventFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public Task<Guid> RaiseEvent(string eventName, string user, XmlDocument doc, CancellationToken token)
        {            
            return Connection.InvokeAsync<Guid>("SendXml", eventName, user, doc, token);
        }

        public Task<Guid> RaiseEvent<T>(string eventName, string user, T json, CancellationToken token)  where  T : new ()     
        {            
            return Connection.InvokeAsync<Guid>("SendJson", eventName, user, json, token);
        }

        public Task<Guid> RaiseEvent(string eventName, string user, byte[] data, CancellationToken token) 
        {            
            return Connection.InvokeAsync<Guid>("SendBinary", eventName, user, data,token);
        }
        
        public Task<Guid> RaiseEvent(string eventName, string user, string data, CancellationToken token) 
        {            
            return Connection.InvokeAsync<Guid>("SendString", eventName, user, data,token);
        }
    }
}