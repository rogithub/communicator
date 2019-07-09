using System;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using Communicator.Rx;

namespace Communicator
{

    public interface IObservableFactory
    {
        IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName) where T: new();
        IObservable<IMessage<string, T>> GetString<T>(string eventName) where T: new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName) where D: new() where M : new();

        IObservable<string> GetOnConnected();
        IObservable<string> GetOnDisconnected();
    }

    internal class ObservableFactory : IObservableFactory
    {
        private HubConnection Connection { get; set; }
        private IStringDeserializer DefaultDeserializer { get; set; }
        public ObservableFactory(HubConnection connection, IStringDeserializer deserializer)
        {
            this.Connection = connection;
            this.DefaultDeserializer = deserializer; 
        }

        public IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName) where T: new()
        {
            return new BinaryObservable<T>(this.Connection, this.DefaultDeserializer, eventName);
        }
        public IObservable<IMessage<string, T>> GetString<T>(string eventName) where T: new()
        {
            return new StringObservable<T>(this.Connection, this.DefaultDeserializer, eventName);
        }
        public IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName) where D: new() where M : new()
        {
            return new StringSerializedObservable<D, M>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<string> GetOnConnected()
        {
            return new ConnectedObservable(this.Connection);
        }
        public IObservable<string> GetOnDisconnected()
        {
            return new DisconnectedObservable(this.Connection);
        }
    }
}
