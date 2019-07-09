using System.Collections.Generic;

namespace Communicator.Core
{
    public class StringMessage<T> : MessageBase<string, T> where T: new()
    {        
        public StringMessage(string data, T metaData): base (data, metaData)
        {

        }

    }

}
