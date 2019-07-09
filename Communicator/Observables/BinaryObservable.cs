using System;
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
            return this.Connection.On<string, byte[]>(EventName, (meta, data) =>
            {
                try 
                {
                    RegisterOnCompleted(observer);
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
}
