using System;
using System.Threading.Tasks;
using Communicator.Core;

namespace Communicator.Obserables
{
    internal abstract class ObservableBase<D, M> : IObservable<IMessage<D, M>>
    {
        protected string EventName { get; set; }
        protected IHubConnection Connection { get; set; }
        protected IStringDeserializer DefaultSerializer { get; set; }  

        protected void RegisterOnCompleted(IObserver<IMessage<D,M>> observer)
        {
            Connection.Closed += (error) =>
            {
                observer.OnCompleted();
                return Task.CompletedTask;
            };
        }
        
        public ObservableBase(IHubConnection connection, IStringDeserializer deserializer, string eventName)
        {
            this.Connection = connection;
            this.DefaultSerializer = deserializer;
            this.EventName = eventName;
        }
        public abstract IDisposable Subscribe(IObserver<IMessage<D, M>> observer);
    }
}