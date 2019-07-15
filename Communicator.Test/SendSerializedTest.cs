using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class SendSerializedTest
    {
        [Fact]
        public void ShouldSendSerialized()
        {
            var serializer = new JsonSerializer();
            Guid eventId = Guid.NewGuid();            
            string eventName = "Person";  
            var message = new Person(){ Name = "Jhon Doe", Age = 55 };
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);
            Func<string, object, object, object, object, Guid> action = (serverAction, o1, o2, o3, o4) => {

                Assert.Equal(EventNames.SendStringTo, serverAction);
                Assert.Equal(eventName, o1);
                Assert.Equal(Array.Empty<string>(), o2);
                Assert.Equal(serializedMetaData, o3);
                Assert.Equal(serializer.Serialize(message), o4);
                return eventId;
            };

            ConnectionMock connection = new ConnectionMock(action);
            EventSender sender = new EventSender(connection, serializer);

            Task<Guid> id = sender.Serialized<Person>(new EventInfo(eventName), message, serializer);
            id = sender.Serialized<Person>(new EventInfo(eventName), new StringSerializedMessage<Person>(message, metaData), serializer);
            id = sender.Serialized<Person, List<KeyValue>>(new EventInfo(eventName), new StringSerializedMessage<Person, List<KeyValue>>(message, metaData), serializer);
            id = sender.Serialized<Person, List<KeyValue>>(new EventInfo(eventName), new StringSerializedMessage<Person, List<KeyValue>>(message, metaData), serializer, serializer);
            
            Assert.Equal(eventId, id.GetAwaiter().GetResult());
        }
    }
}
