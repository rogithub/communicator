using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public interface IHandlerFactory
    {
        IDisposable OnConnected(Action<string> action);
        IDisposable OnDisconnected(Action<string> actionConnectionId);
        IDisposable Binary(string eventName, Action<MetaData, byte[]> action);
        IDisposable Xml(string eventName, Action<MetaData, XmlDocument> action);
        IDisposable String(string eventName, Action<MetaData, string> action);
        IDisposable Json<T>(string eventName, Action<MetaData, T> action);
    }

    internal class HandlerFactory : IHandlerFactory
    {
        private HubConnection Connection { get; set; }
        public HandlerFactory(HubConnection connection)
        {
            this.Connection = connection;
        }

        public IDisposable Json<T>(string eventName, Action<MetaData, T> action)
        {
            return this.Connection.On<string, string>(eventName, (meta, json) =>
            {
                MetaData metaData = SerializationJson.Deserialize<MetaData>(meta);
                T data = SerializationJson.Deserialize<T>(json);
                action(metaData, data);
            });
        }

        public IDisposable Binary(string eventName, Action<MetaData, byte[]> action)
        {
            return this.Connection.On<string, byte[]>(eventName, (meta, data) =>
            {
                MetaData metaData = SerializationJson.Deserialize<MetaData>(meta);
                action(metaData, data);
            });
        }

        public IDisposable Xml(string eventName, Action<MetaData, XmlDocument> action)
        {
            return this.Connection.On<string, XmlDocument>(eventName, (meta, data) =>
            {
                MetaData metaData = SerializationJson.Deserialize<MetaData>(meta);
                action(metaData, data);
            });
        }

        public IDisposable String(string eventName, Action<MetaData, string> action) 
        {
            return this.Connection.On<string, string>(eventName, (meta, data) =>
            {
                MetaData metaData = SerializationJson.Deserialize<MetaData>(meta);
                action(metaData, data);
            });
            
        }

        public IDisposable OnDisconnected(Action<string> actionConnectionId)
        {
            return this.Connection.On<string>(EventNames.OnDisconnected, actionConnectionId);
        }

        public IDisposable OnConnected(Action<string> actionConnectionId)
        {
            return this.Connection.On<string>(EventNames.OnConnected, actionConnectionId);
        }
    }
}
