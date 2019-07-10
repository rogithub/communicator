using System;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Obserables
{
    internal class ConnectedObservable<T> : ObservableBase<string, T> where T: new()
    {                
        public ConnectedObservable(HubConnection connection, IStringDeserializer serializer)
        : base(connection, serializer, EventNames.OnConnected)
        {
            
        }             
        
        public override IDisposable Subscribe(IObserver<IMessage<string, T>> observer)
        {               
            return this.Connection.On<string, string>(EventNames.OnConnected, (meta, data) =>
            {
                try 
                {                    
                    T metaData = DefaultSerializer.Deserialize<T>(meta);
                    
                    IMessage<string, T> message = new StringMessage<T>(data, metaData);
                    
                    observer.OnNext(message); 
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }
    }

}
