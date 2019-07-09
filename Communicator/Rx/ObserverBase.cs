using System;
using System.Collections.Generic;
using System.Linq;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Rx
{
    internal abstract class ObserverBase<D, M> : IObserver<IMessage<D, M>>
    {
        protected string EventName { get; set; }
        protected IEnumerable<string> To { get; set; }
        protected HubConnection Connection { get; set; }
        protected IStringSerializer DefaultSerializer { get; set; }
        protected Action OnCompletedAction { get; set; }
        protected Action<Exception> OnErrorAction { get; set; }
        
        public ObserverBase(HubConnection connection, IStringSerializer serializer, string eventName, IEnumerable<string> to, Action onCompleted, Action<Exception> onError)
        {
            this.Connection = connection;
            this.DefaultSerializer = serializer;
            this.EventName = eventName;
            this.To = to;

            this.OnCompletedAction = onCompleted;
            this.OnErrorAction = onError;
        }

        public void OnCompleted()
        {
            this.OnCompletedAction();
        }

        public void OnError(Exception error)
        {
            this.OnErrorAction(error);
        }

        public abstract void OnNext(IMessage<D, M> value);
    }

}
