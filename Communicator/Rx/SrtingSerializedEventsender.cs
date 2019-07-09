using System;
using System.Collections.Generic;
using System.Linq;
using Communicator.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator.Rx
{
    internal class StringSerializedEventSender<D, M> : EventSenderBase<D, M> where D: new() where M : new()
    {                
        public StringSerializedEventSender(HubConnection connection, IStringSerializer serializer, string eventName, IEnumerable<string> to, Action onCompleted, Action<Exception> onError)
        : base(connection, serializer, eventName, to, onCompleted, onError)
        {
            
        }            
        public override void OnNext(IMessage<D, M> value)
        {
            Connection.InvokeAsync<Guid>(EventNames.SendStringTo, To.ToArray(), EventName, 
            DefaultSerializer.Serialize(value.MetaData), 
            DefaultSerializer.Serialize(value.Data));
        }
    }

}
