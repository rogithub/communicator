using System.Collections.Generic;

namespace Communicator.Core
{
    public class NoMetaDataStringMessage : StringMessage<List<KeyValue>>
    {        
        public NoMetaDataStringMessage(string data): base (data, new List<KeyValue>())
        {

        }
    }

    public class NoMetaDataSerializedMessage<T> : StringSerializedMessage<T, List<KeyValue>> where T: new()
    {        
        public NoMetaDataSerializedMessage(T data): base (data,  new List<KeyValue>())
        {
            
        }        
    }

    public class NoMetaDataBinaryMessage : BinaryMessage<List<KeyValue>>
    {                
        public NoMetaDataBinaryMessage(byte[] data): base (data, new List<KeyValue>())
        {
            
        }        
    }

}