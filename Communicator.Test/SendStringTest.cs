using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class SendStringTest
    {
        [Fact]
        public void ShouldSendString()
        {            
            var serializer = new JsonSerializer();
            Guid eventId = Guid.NewGuid();
            string eventName = "Chat";
            string message = "Hola";            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);            
            string to = "Bob";
            var info = new EventInfo(eventName, to, string.Empty);

            Func<string, object, object, object, object, Guid> action = (serverAction, o1, o2, o3, o4) => {

                Assert.Equal(EventNames.SendStringTo, serverAction);
                Assert.Equal(eventName, o1);
                Assert.Equal(info.To, o2);                
                Assert.Equal(serializedMetaData, o3);
                Assert.Equal(message, o4);
                return eventId;
            };

            ConnectionMock connection = new ConnectionMock(action);
            EventSender sender = new EventSender(connection, serializer);

            Task<Guid> id = sender.String(info, message);
            id = sender.String(info, new StringMessage(message, metaData));
            id = sender.String(info, new StringMessage(message, metaData), serializer);            
            
            Assert.Equal(eventId, id.GetAwaiter().GetResult());
        }        
    }
}
