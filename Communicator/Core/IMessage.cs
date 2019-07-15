
namespace Communicator.Core
{
    public interface IMessage<D, M>
    {        
        D Data { get; set; }
        M MetaData { get; set; }                
    }

}