
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;

namespace Communicator.Obserables
{
    internal class StringSerializedObservable<D, M> : ObservableBase<D, M> where D: new() where M : new()
    {                
        public StringSerializedObservable(HubConnection connection, IStringDeserializer serializer, string eventName)
        : base(connection, serializer, eventName)
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

    internal class StringSerializedObservable<T> : ObservableBase<T, List<MetaData>> where T: new()
    {                
        public StringSerializedObservable(HubConnection connection, IStringDeserializer serializer, string eventName)
        : base(connection, serializer, eventName)
        {
            
        }            
        
        public override IDisposable Subscribe(IObserver<IMessage<T, List<MetaData>>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<List<MetaData>, string>(EventName, (metaData, data) =>
            {
                try 
                {                    
                    T deserilized = DefaultSerializer.Deserialize<T>(data);

                    IMessage<T, List<MetaData>> message = new StringSerializedMessage<T, List<MetaData>>(deserilized, metaData);
                    
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
