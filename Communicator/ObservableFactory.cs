using System;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using Communicator.Obserables;
using System.Collections.Generic;

namespace Communicator
{

    public interface IObservableFactory
    {
        IObservable<string> GetOnDisconnected();
        IObservable<string> GetOnConnected();
        IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName, IStringDeserializer dataDeserializer) where T: new();
        IObservable<IMessage<string, T>> GetString<T>(string eventName, IStringDeserializer dataDeserializer) where T: new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer) where D: new() where M : new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer) where D: new() where M : new();
        
        IObservable<IMessage<byte[], List<KeyValue>>> GetBinary(string eventName);
        IObservable<IMessage<string, List<KeyValue>>> GetString(string eventName);
        IObservable<IMessage<T, List<KeyValue>>> GetSerialized<T>(string eventName, IStringDeserializer dataDeserializer) where T : new();        
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

        public IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName, IStringDeserializer dataDeserializer) where T: new()
        {
            return new BinaryObservable<T>(this.Connection, dataDeserializer, eventName);
        }
        public IObservable<IMessage<string, T>> GetString<T>(string eventName, IStringDeserializer dataDeserializer) where T: new()
        {
            return new StringObservable<T>(this.Connection, dataDeserializer, eventName);
        }
        public IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer) where D: new() where M : new()
        {
            return new StringSerializedObservable<D, M>(this.Connection, dataDeserializer, eventName);
        }

        public IObservable<string> GetOnDisconnected()
        {
            return new DisconnectedObservable(this.Connection);
        }
        public IObservable<string> GetOnConnected()
        {
            return new ConnectedObservable(this.Connection);
        }

        public IObservable<IMessage<byte[], List<KeyValue>>> GetBinary(string eventName)
        {            
            return new BinaryObservable<List<KeyValue>>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<IMessage<string, List<KeyValue>>> GetString(string eventName)
        {
            return new StringObservable<List<KeyValue>>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<IMessage<T, List<KeyValue>>> GetSerialized<T>(string eventName, IStringDeserializer dataDeserializer) where T : new()
        {
            return new CustomSerializedObservable<T, List<KeyValue>>(this.Connection, eventName, dataDeserializer, DefaultDeserializer);
        }       

        public IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer)
            where D : new()
            where M : new()
        {
            return new CustomSerializedObservable<D, M>(this.Connection, eventName, dataDeserializer, metaDeserializer);
        }        
    }
}