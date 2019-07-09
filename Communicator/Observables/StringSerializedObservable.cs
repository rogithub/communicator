
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;

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
            return this.Connection.On<string, string>(EventName, (meta, data) =>
            {
                try 
                {
                    RegisterOnCompleted(observer);
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

}
