using System.Collections.Generic;

namespace Communicator.Core
{
    public class NoMetaDataStringMessage : StringMessage<List<MetaData>>
    {        
        public NoMetaDataStringMessage(string data): base (data, new List<MetaData>())
        {

        }
    }

    public class NoMetaDataSerializedMessage<T> : StringSerializedMessage<T, List<MetaData>> where T: new()
    {        
        public NoMetaDataSerializedMessage(T data): base (data,  new List<MetaData>())
        {
            
        }        
    }

    public class NoMetaDataBinaryMessage : BinaryMessage<List<MetaData>>
    {                
        public NoMetaDataBinaryMessage(byte[] data): base (data, new List<MetaData>())
        {
            
        }        
    }

}
