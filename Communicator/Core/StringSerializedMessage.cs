using System.Collections.Generic;


namespace Communicator.Core
{
    public class StringSerializedMessage<D, M> : MessageBase<D, M> where D: new() where M: new()
    {        
        public StringSerializedMessage(D data, M metaData): base (data, metaData)
        {
            
        }        
    }

    public class StringSerializedMessage<T> : StringSerializedMessage<T, List<MetaData>> where T: new()
    {        
        public StringSerializedMessage(T data, List<MetaData> metaData): base (data, metaData)
        {
            
        }        
    }

}
