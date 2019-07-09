using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Obserables
{
    internal class ConnectedObservable : IObservable<string>
    {                
        protected HubConnection Connection { get; set; }
        public ConnectedObservable(HubConnection connection)        
        {
            this.Connection = connection;
        }            
        public IDisposable Subscribe(IObserver<string> observer)
        {            
            return this.Connection.On<string>(EventNames.OnConnected, connectionId => {
                observer.OnNext(connectionId);
            });
        } 
    }

}
