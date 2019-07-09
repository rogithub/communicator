using System.Linq;

namespace CommunicatorRx
{
    public interface IMessage<D, M>
    {
        D Data { get; set; }
        M MetaData { get; set; }
        string EventName { get; set; }
    }

    public interface IMessageTo<D, M> : IMessageTo<D, M>
    {
        // Empty means to all
        IEnummerable<string> To { get; set; }
    }
}
