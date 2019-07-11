using System;
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
}