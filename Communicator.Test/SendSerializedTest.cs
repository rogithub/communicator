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
            string serializedData = serializer.Serialize(message);
            string to = "Bob";
            var info = new EventInfo(eventName, to, string.Empty);
            int serializationCounter = 0;

            Func<string, object, object, object, object, Guid> action = (serverAction, o1, o2, o3, o4) => {

                Assert.Equal(EventNames.SendStringTo, serverAction);
                Assert.Equal(eventName, o1);
                Assert.Equal(info.To, o2);
                Assert.Equal(serializedMetaData, o3);
                Assert.Equal(serializer.Serialize(message), o4);
                return eventId;
            };

            Action<object,  string> onDataSerialized = ( cSharpObj, json ) => {                                
                Assert.Equal(serializedData, json);
                Assert.True(cSharpObj is Person);
            };
            var dataSerializer = new SerializationMock(onDataSerialized);

            Action<object,  string> onMetaSerialized = ( cSharpObj, json ) => {                                
                Assert.Equal(serializedMetaData, json);
                Assert.True(cSharpObj is List<KeyValue>);
            };
            var metaSerializer = new SerializationMock(onMetaSerialized);

            Action<object, string> bothAction = ( cSharpObj, json ) => {
                if (cSharpObj is Person)
                {
                    Assert.Equal(serializedData, json);                    
                }
                if (cSharpObj is List<KeyValue>)
                {
                    Assert.Equal(serializedMetaData, json);
                }
                Assert.True(cSharpObj is List<KeyValue> || cSharpObj is Person);

                serializationCounter ++;
            };
            var bothSerializer = new SerializationMock(bothAction);

            ConnectionMock connection = new ConnectionMock(action);
            EventSender sender = new EventSender(connection, serializer);

            Task<Guid> id = sender.Serialized<Person>(info, message, dataSerializer);
            id = sender.Serialized<Person>(info, new StringSerializedMessage<Person>(message, metaData), dataSerializer);
            id = sender.Serialized<Person, List<KeyValue>>(info, new StringSerializedMessage<Person, List<KeyValue>>(message, metaData), bothSerializer);
            id = sender.Serialized<Person, List<KeyValue>>(info, new StringSerializedMessage<Person, List<KeyValue>>(message, metaData), dataSerializer, metaSerializer);
            
            Assert.Equal(eventId, id.GetAwaiter().GetResult());
            Assert.Equal(2, serializationCounter);
        }
    }
}
