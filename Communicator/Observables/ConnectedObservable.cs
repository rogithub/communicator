using System;

namespace Communicator.Obserables
{
    internal class ConnectedObservable : IObservable<string>
    {                
        protected IHubConnection Connection { get; set; }
        public ConnectedObservable(IHubConnection connection)
        {
            this.Connection = connection;
        }            
        public IDisposable Subscribe(IObserver<string> observer)
        {            
            return this.Connection.On<string>(EventNames.OnConnected, connectionId => {
                try
                {
                    observer.OnNext(connectionId);
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