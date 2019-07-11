using System;
using Microsoft.AspNetCore.SignalR.Client;
using Communicator.Core;
using Communicator.Obserables;
using System.Collections.Generic;

namespace Communicator
{

    public interface IObservableFactory
    {
        IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName) where T: new();
        IObservable<IMessage<string, T>> GetString<T>(string eventName) where T: new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName) where D: new() where M : new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer) where D: new() where M : new();
        
        IObservable<IMessage<string, T>> GetOnConnected<T>() where T: new();

        IObservable<IMessage<byte[], List<MetaData>>> GetBinary(string eventName);
        IObservable<IMessage<string, List<MetaData>>> GetString(string eventName);
        IObservable<IMessage<T, List<MetaData>>> GetSerialized<T>(string eventName) where T : new();
        IObservable<IMessage<T, List<MetaData>>> GetSerialized<T>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer) where T: new();
        IObservable<IMessage<string, List<MetaData>>> GetOnConnected();

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

        public IObservable<string> GetOnDisconnected()
        {
            return new DisconnectedObservable(this.Connection);
        }

        public IObservable<IMessage<string, T>> GetOnConnected<T>() where T: new()
        {
            return new ConnectedObservable<T>(this.Connection, this.DefaultDeserializer);
        }

        public IObservable<IMessage<byte[], List<MetaData>>> GetBinary(string eventName)
        {            
            return new BinaryObservable<List<MetaData>>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<IMessage<string, List<MetaData>>> GetString(string eventName)
        {
            return new StringObservable<List<MetaData>>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<IMessage<T, List<MetaData>>> GetSerialized<T>(string eventName) where T : new()
        {
            return new StringSerializedObservable<T, List<MetaData>>(this.Connection, this.DefaultDeserializer, eventName);
        }

        public IObservable<IMessage<string, List<MetaData>>> GetOnConnected()
        {
            return new ConnectedObservable(this.Connection);
        }

        public IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer)
            where D : new()
            where M : new()
        {
            return new CustomSerializedObservable<D, M>(this.Connection, eventName, dataDeserializer, metaDeserializer);
        }

        public IObservable<IMessage<T, List<MetaData>>> GetSerialized<T>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer) where T : new()
        {
            return new CustomSerializedObservable<T, List<MetaData>>(this.Connection, eventName, dataDeserializer, metaDeserializer);
        }
    }
}
