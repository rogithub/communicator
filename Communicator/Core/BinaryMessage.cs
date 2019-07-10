using System.Collections.Generic;


namespace Communicator.Core
{
    public class BinaryMessage<T> : MessageBase<byte[], T> where T : new()
    {                
        public BinaryMessage(byte[] data, T metaData): base (data, metaData)
        {
            
        }        
    }

    public class BinaryMessage : BinaryMessage<List<MetaData>>
    {                
        public BinaryMessage(byte[] data, List<MetaData> metaData): base (data, metaData)
        {
            
        }        
    }

}
