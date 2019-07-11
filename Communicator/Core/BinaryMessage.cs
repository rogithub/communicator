using System.Collections.Generic;


namespace Communicator.Core
{
    public class BinaryMessage<T> : MessageBase<byte[], T> where T : new()
    {                
        public BinaryMessage(byte[] data, T metaData): base (data, metaData)
        {
            
        }        
    }

    public class BinaryMessage : BinaryMessage<List<KeyValue>>
    {                
        public BinaryMessage(byte[] data, List<KeyValue> metaData): base (data, metaData)
        {
            
        }        
    }

}
