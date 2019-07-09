using System.Collections.Generic;

namespace Communicator.Rx
{
    public class StringMessage<T> : MessageBase<string, T> where T: new()
    {        
        public StringMessage(string data, T metaData): base (data, metaData)
        {

        }

    }

}
