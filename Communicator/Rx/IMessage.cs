using System.Collections.Generic;
using System.Linq;

namespace Communicator.Rx
{
    public interface IMessage<D, M>
    {        
        D Data { get; set; }
        M MetaData { get; set; }                
    }

}
