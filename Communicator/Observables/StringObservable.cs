using System;
using System.Collections.Generic;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Obserables
{
    internal class StringObservable<T> : ObservableBase<string, T> where T: new()
    {                
        public StringObservable(HubConnection connection, IStringDeserializer serializer, string eventName)
        : base(connection, serializer, eventName)
        {
            
        }            
        public override IDisposable Subscribe(IObserver<IMessage<string, T>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<string, string>(EventName, (meta, data) =>
            {
                try
                {                    
                    T metaData = DefaultSerializer.Deserialize<T>(meta);

                    IMessage<string, T> message = new StringMessage<T>(data, metaData);
                    
                    observer.OnNext(message);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        } 
    }

    internal class StringObservable : ObservableBase<string, List<MetaData>>
    {                
        public StringObservable(HubConnection connection, string eventName)
        : base(connection, null, eventName)
        {
            
        }            
        public override IDisposable Subscribe(IObserver<IMessage<string, List<MetaData>>> observer)
        {        
            RegisterOnCompleted(observer);
                
            return this.Connection.On<List<MetaData>, string>(EventName, (metaData, data) =>
            {
                try
                {                    
                    IMessage<string, List<MetaData>> message = new StringMessage<List<MetaData>>(data, metaData);
                    
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
