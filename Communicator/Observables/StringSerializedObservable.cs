using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Communicator.Obserables
{
    internal class StringSerializedObservable<D, M> : ObservableBase<D, M> where D: new() where M : new()
    {                        
        public StringSerializedObservable(HubConnection connection, IStringDeserializer deserializer, string eventName)
        : base(connection, deserializer, eventName)
        {

        }            
        
        public override IDisposable Subscribe(IObserver<IMessage<D, M>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<string, string>(EventName, (meta, data) =>
            {
                try 
                {                    
                    M metaData = DefaultSerializer.Deserialize<M>(meta);
                    D deserilized = DefaultSerializer.Deserialize<D>(data);

                    IMessage<D, M> message = new StringSerializedMessage<D, M>(deserilized, metaData);
                    
                    observer.OnNext(message);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        } 
    }

    internal class CustomSerializedObservable<D, M> : ObservableBase<D, M> where D: new() where M : new()
    {                
        private IStringDeserializer MetaDeserializer { get; set; }
        public CustomSerializedObservable(HubConnection connection, string eventName, IStringDeserializer dataDeserializer, IStringDeserializer metaDeserializer)
        : base(connection, dataDeserializer, eventName)
        {
            this.MetaDeserializer = metaDeserializer;
        }            
        
        public override IDisposable Subscribe(IObserver<IMessage<D, M>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<string, string>(EventName, (meta, data) =>
            {
                try 
                {                    
                    M metaData = MetaDeserializer.Deserialize<M>(meta);
                    D deserilized = DefaultSerializer.Deserialize<D>(data);

                    IMessage<D, M> message = new StringSerializedMessage<D, M>(deserilized, metaData);
                    
                    observer.OnNext(message);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        } 
    }
}