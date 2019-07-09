using System;
using System.Collections.Generic;
using System.Linq;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Rx
{
    internal class BinaryEventSender<T> : EventSenderBase<T, byte[]> 
    {
       
        public BinaryEventSender(HubConnection connection, IStringSerializer serializer, string eventName, IEnumerable<string> to, Action onCompleted, Action<Exception> onError)
        : base(connection, serializer, eventName, to, onCompleted, onError)
        {
            
        }            
        public override void OnNext(IMessage<T, byte[]> value)
        {
            Connection.InvokeAsync<Guid>(EventNames.SendStringTo, To.ToArray(), this.EventName, DefaultSerializer.Serialize(value.MetaData), value.Data);
        }        
    }
}
