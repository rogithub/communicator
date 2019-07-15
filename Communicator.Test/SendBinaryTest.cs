using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class SendBinaryTest
    {
        [Fact]
        public void ShouldSendBinary()
        {            
            var serializer = new JsonSerializer();
            Guid eventId = Guid.NewGuid();
            string eventName = "Binary";
            byte[] message = new byte[] {1, 2, 3};
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);
            Func<string, object, object, object, object, Guid> action = (serverAction, o1, o2, o3, o4) => {

                Assert.Equal(EventNames.SendBinaryTo, serverAction);
                Assert.Equal(eventName, o1);
                Assert.Equal(Array.Empty<string>(), o2);
                Assert.Equal(serializedMetaData, o3);
                Assert.Equal(message, o4);
                return eventId;
            };

            ConnectionMock connection = new ConnectionMock(action);
            EventSender sender = new EventSender(connection, serializer);

            Task<Guid> id = sender.Binary(new EventInfo(eventName), message);
            id = sender.Binary(new EventInfo(eventName), new BinaryMessage(message, metaData));
            id = sender.Binary(new EventInfo(eventName), new BinaryMessage(message, metaData), serializer);
            
            Assert.Equal(eventId, id.GetAwaiter().GetResult());
        }
    }
}
