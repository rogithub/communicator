using System;
using System.Threading.Tasks;
using Communicator.Core;

namespace Communicator
{
    public interface IEventSender
    {        
        Task<Guid> Binary<M>(EventInfo info, BinaryMessage<M> message, IStringSerializer dataSerializer) where M : new();
        Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message, IStringSerializer dataSerializer) where D : new() where M : new();
        Task<Guid> Serialized<D, M>(EventInfo info, StringSerializedMessage<D, M> message, IStringSerializer dataSerializer , IStringSerializer metaSerializer) where D : new() where M : new();
        Task<Guid> String<M>(EventInfo info, StringMessage<M> message, IStringSerializer dataSerializer) where M : new();        

        Task<Guid> Binary(EventInfo info, byte[] message);
        Task<Guid> Serialized<T>(EventInfo info, T message, IStringSerializer dataSerializer) where T : new();
        Task<Guid> String(EventInfo info, string message);

        Task<Guid> Binary(EventInfo info, BinaryMessage message);
        Task<Guid> Serialized<T>(EventInfo info, StringSerializedMessage<T> message, IStringSerializer dataSerializer) where T : new();        
        Task<Guid> String(EventInfo info, StringMessage message);
    }    
}