using System.Collections.Generic;
using System.Linq;

namespace Communicator.Core
{
    public interface IMessage<D, M>
    {        
        D Data { get; set; }
        M MetaData { get; set; }                
    }

}