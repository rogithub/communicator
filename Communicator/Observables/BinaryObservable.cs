using System;
using System.Collections.Generic;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Obserables
{
    internal class BinaryObservable<T> : ObservableBase<byte[], T> where T: new()
    {
       
        public BinaryObservable(HubConnection connection, IStringDeserializer serializer, string eventName)
        : base(connection, serializer, eventName)
        {
            
        }            
        public override IDisposable Subscribe(IObserver<IMessage<byte[], T>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<string, byte[]>(EventName, (meta, data) =>
            {
                try 
                {                    
                    T metaData = DefaultSerializer.Deserialize<T>(meta);

                    IMessage<byte[], T> message = new BinaryMessage<T>(data, metaData);
                    
                    observer.OnNext(message);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }      
    }

    internal class BinaryObservable : ObservableBase<byte[], List<MetaData>>
    {
       
        public BinaryObservable(HubConnection connection, string eventName)
        : base(connection, null, eventName)
        {
            
        }            
        public override IDisposable Subscribe(IObserver<IMessage<byte[], List<MetaData>>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<List<MetaData>, byte[]>(EventName, (metaData, data) =>
            {
                try 
                {                                        
                    IMessage<byte[], List<MetaData>> message = new BinaryMessage<List<MetaData>>(data, metaData);
                    
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
