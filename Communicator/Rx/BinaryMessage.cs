using System.Collections.Generic;


namespace Communicator.Rx
{
    public class BinaryMessage<T> : MessageBase<byte[], T> where T : new()
    {                
        public BinaryMessage(byte[] data, T metaData): base (data, metaData)
        {
            
        }        
    }

}
