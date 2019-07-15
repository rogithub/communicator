using System.Threading.Tasks;

namespace Communicator
{
    public interface IEventSource 
    {
        IObservableFactory GetObservablesFactory();
        IEventSender GetEventSender();
        Task<string> GetConnectionId();
        Task Connect();
    }   
}