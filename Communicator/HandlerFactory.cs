using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public interface IHandlerFactory
    {
        IDisposable AddHandler(string eventName, Action<string, byte[]> action);
        IDisposable AddHandler(string eventName, Action<string, XmlDocument> action);
        IDisposable AddHandler<T>(string eventName, Action<string, T> action) where T : new();
        IDisposable AddHandler(string eventName, Action<string, string> action) ;    
    }

    internal class HandlerFactory : IHandlerFactory
    {
        private HubConnection Connection { get; set; }
        public HandlerFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public IDisposable AddHandler(string eventName, Action<string, byte[]> action)
        {
            return this.Connection.On<string, byte[]>(eventName, (user, message) => {				
                action(user, message);
			});
        }

        public IDisposable AddHandler(string eventName, Action<string, XmlDocument> action)
        {
            return this.Connection.On<string, XmlDocument>(eventName, (user, message) => {				
                action(user, message);
			});
        }

        public IDisposable AddHandler<T>(string eventName, Action<string, T> action) where T : new()
        {
            return this.Connection.On<string, T>(eventName, (user, message) => {				
                action(user, message);
			});
        }

        public IDisposable AddHandler(string eventName, Action<string, string> action) 
        {
            return this.Connection.On<string, string>(eventName, (user, message) => {				
                action(user, message);
			});
        }
    }
}