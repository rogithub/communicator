using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Obserables
{
    internal class DisconnectedObservable : IObservable<string>
    {                
        protected HubConnection Connection { get; set; }
        public DisconnectedObservable(HubConnection connection)        
        {
            this.Connection = connection;
        }            
        public IDisposable Subscribe(IObserver<string> observer)
        {            
            return this.Connection.On<string>(EventNames.OnDisconnected, connectionId => {
                observer.OnNext(connectionId);
                observer.OnCompleted();
            });
        } 
    }

}
