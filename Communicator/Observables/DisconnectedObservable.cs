using System;

namespace Communicator.Obserables
{
    internal class DisconnectedObservable : IObservable<string>
    {                
        protected IHubConnection Connection { get; set; }
        public DisconnectedObservable(IHubConnection connection)        
        {
            this.Connection = connection;
        }            
        public IDisposable Subscribe(IObserver<string> observer)
        {            
            return this.Connection.On<string>(EventNames.OnDisconnected, connectionId => {
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