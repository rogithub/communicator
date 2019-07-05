using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public interface IHandlerFactory
    {
        IDisposable Binary(string eventName, Action<string, byte[]> action);
        IDisposable Xml(string eventName, Action<string, XmlDocument> action);
        IDisposable String(string eventName, Action<string, string> action) ;    
    }

    internal class HandlerFactory : IHandlerFactory
    {
        private HubConnection Connection { get; set; }
        public HandlerFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public IDisposable Binary(string eventName, Action<string, byte[]> action)
        {
            return this.Connection.On<string, byte[]>(eventName, (user, message) => {				
                action(user, message);
			});
        }

        public IDisposable Xml(string eventName, Action<string, XmlDocument> action)
        {
            return this.Connection.On<string, XmlDocument>(eventName, (user, message) => {				
                action(user, message);
			});
        }

        public IDisposable String(string eventName, Action<string, string> action) 
        {
            return this.Connection.On<string, string>(eventName, (user, message) => {				
                action(user, message);
			});
        }
    }
}