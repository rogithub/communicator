using System;
using Communicator.Core;
using System.Collections.Generic;

namespace Communicator
{
    public interface IObservableFactory
    {
        IObservable<string> GetOnDisconnected();
        IObservable<string> GetOnConnected();
        IObservable<IMessage<byte[], T>> GetBinary<T>(string eventName, IStringDeserializer dataDeserializer) where T: new();
        IObservable<IMessage<string, T>> GetString<T>(string eventName, IStringDeserializer dataDeserializer) where T: new();        
        IObservable<IMessage<byte[], List<KeyValue>>> GetBinary(string eventName);
        IObservable<IMessage<string, List<KeyValue>>> GetString(string eventName);
        
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer) where D: new() where M : new();
        IObservable<IMessage<D, M>> GetSerialized<D, M>(string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer) where D: new() where M : new();
        IObservable<IMessage<T, List<KeyValue>>> GetSerialized<T>(string eventName, IStringDeserializer dataDeserializer) where T : new();
        
    }
    
}