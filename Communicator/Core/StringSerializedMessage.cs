using System.Collections.Generic;


namespace Communicator.Core
{
    public class StringSerializedMessage<D, M> : MessageBase<D, M> where D: new() where M: new()
    {        
        public StringSerializedMessage(D data, M metaData): base (data, metaData)
        {
            
        }        
    }

}
