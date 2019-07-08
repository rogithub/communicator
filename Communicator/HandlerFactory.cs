using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{

    public interface IHandlerFactory<TMetaData>
    {
        IDisposable OnConnected(Action<string> getConnId);
        IDisposable OnDisconnected(Action<string> getConnId);
        IDisposable Binary(string eventName, Action<TMetaData, byte[]> getData);
        IDisposable String(string eventName, Action<TMetaData, string> getData);
        IDisposable Json<T>(string eventName, Action<TMetaData, T> getData);
        IDisposable Serialized<T>(string eventName, Func<string, T> deserializer, Action<TMetaData, T> getData);
    }

    internal class HandlerFactory<TMetaData> : IHandlerFactory<TMetaData>
    {
        private HubConnection Connection { get; set; }
        private IStringDeserializer DefaultDeserializer { get; set; }
        public HandlerFactory(HubConnection connection, IStringDeserializer deserializer)
        {
            this.Connection = connection;
            this.DefaultDeserializer = deserializer; 
        }

        public IDisposable Json<T>(string eventName, Action<TMetaData, T> getData)
        {
            return this.Serialized(eventName, d => DefaultDeserializer.Deserialize<T>(d), getData);
        }

        public IDisposable Serialized<T>(string eventName, Func<string, T> deserializer, Action<TMetaData, T> getData)
        {
            return this.Connection.On<string, string>(eventName, (meta, json) =>
            {
                TMetaData metaData = DefaultDeserializer.Deserialize<TMetaData>(meta);
                T data = deserializer(json);
                getData(metaData, data);
            });
        }

        public IDisposable Binary(string eventName, Action<TMetaData, byte[]> getData)
        {
            return this.Connection.On<string, byte[]>(eventName, (meta, data) =>
            {
                TMetaData metaData = DefaultDeserializer.Deserialize<TMetaData>(meta);
                getData(metaData, data);
            });
        }
        
        public IDisposable String(string eventName, Action<TMetaData, string> getData) 
        {
            return this.Connection.On<string, string>(eventName, (meta, data) =>
            {
                TMetaData metaData = DefaultDeserializer.Deserialize<TMetaData>(meta);
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
