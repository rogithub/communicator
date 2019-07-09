using System.Collections.Generic;
using System.Linq;

namespace Communicator.Rx
{
    internal interface IMessage<D, M>
    {        
        D Data { get; set; }
        M MetaData { get; set; }                
    }

}
