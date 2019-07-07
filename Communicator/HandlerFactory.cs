using System.Xml;
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public interface IHandlerFactory
    {
        IDisposable OnConnected(Action<string> getConnId);
        IDisposable OnDisconnected(Action<string> getConnId);
        IDisposable Binary(string eventName, Action<MetaData, byte[]> getData);
        IDisposable Xml(string eventName, Action<MetaData, XmlDocument> getData);
        IDisposable String(string eventName, Action<MetaData, string> getData);
        IDisposable Json<T>(string eventName, Action<MetaData, T> getData);
        IDisposable Serialized<T>(string eventName, IStringDeserializer serializer, Action<MetaData, T> getData);
    }

    internal class HandlerFactory : IHandlerFactory
    {
        private HubConnection Connection { get; set; }
        private IStringDeserializer JsonSerializer { get; set; }
        public HandlerFactory(HubConnection connection)
        {
            this.Connection = connection;
            this.JsonSerializer = new DefaultJsonSerializer(); 
        }

        public IDisposable Json<T>(string eventName, Action<MetaData, T> getData)
        {
            return this.Serialized(eventName, JsonSerializer, getData);
        }

        public IDisposable Serialized<T>(string eventName, IStringDeserializer serializer, Action<MetaData, T> getData)
        {
            return this.Connection.On<string, string>(eventName, (meta, json) =>
            {
                MetaData metaData = serializer.Deserialize<MetaData>(meta);
                T data = serializer.Deserialize<T>(json);
                getData(metaData, data);
            });
        }

        public IDisposable Binary(string eventName, Action<MetaData, byte[]> getData)
        {
            return this.Connection.On<string, byte[]>(eventName, (meta, data) =>
            {
                MetaData metaData = JsonSerializer.Deserialize<MetaData>(meta);
                getData(metaData, data);
            });
        }

        public IDisposable Xml(string eventName, Action<MetaData, XmlDocument> getData)
        {
            return this.Connection.On<string, XmlDocument>(eventName, (meta, data) =>
            {
                MetaData metaData = JsonSerializer.Deserialize<MetaData>(meta);
                getData(metaData, data);
            });
        }

        public IDisposable String(string eventName, Action<MetaData, string> getData) 
        {
            return this.Connection.On<string, string>(eventName, (meta, data) =>
            {
                MetaData metaData = JsonSerializer.Deserialize<MetaData>(meta);
                getData(metaData, data);
            });            
        }

        public IDisposable OnDisconnected(Action<string> getConnId)
        {
            return this.Connection.On<string>(EventNames.OnDisconnected, getConnId);
        }

        public IDisposable OnConnected(Action<string> getConnId)
        {
            return this.Connection.On<string>(EventNames.OnConnected, getConnId);
        }
    }
}
