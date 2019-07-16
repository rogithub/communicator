using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class ObservableSerializedTest
    {
        [Fact]
        public void GetSerialized()
        {
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };
            var metaData = new List<KeyValue>();
            
            var serializer = new JsonSerializer();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);

            Action<string,  object> onDeserialized = ( json, cSharpObj ) => {                                
                Assert.Equal(serializedData, json);
                Assert.True(cSharpObj is Person);
            };
            var customSerializer = new SerializationMock(onDeserialized);
                       
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
        
            // Not sending Default MetaData            
            var observable = factory.GetSerialized<Person>(eventName, customSerializer);

            observable.Subscribe(msg => {
				Assert.Equal(message.Name, msg.Data.Name);
                Assert.Equal(message.Age, msg.Data.Age);
                Assert.Equal(metaData, msg.MetaData);
			});
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, string>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, serializedData);
        }        

        [Fact]
        public void GetSerializedGenericMetaData()
        {            
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };
            var metaData = new List<KeyValue>();
            
            var serializer = new JsonSerializer();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);

            int serializationCounter = 0;

            Action<string,  object> onDeserialized = ( json, cSharpObj ) => {
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
            var customSerializer = new SerializationMock(onDeserialized);
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Sending generic MetaData
            var observable = factory.GetSerialized<Person, List<KeyValue>>(eventName, customSerializer);

            observable.Subscribe(msg => {
				Assert.Equal(message.Name, msg.Data.Name);
                Assert.Equal(message.Age, msg.Data.Age);
                Assert.Equal(metaData, msg.MetaData);
			});
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, string>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, serializedData);

            Assert.Equal(2, serializationCounter);
        }


        [Fact]
        public void GetSerializedDataAndMeta()
        {            
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };
            var metaData = new List<KeyValue>();
            
            var serializer = new JsonSerializer();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);

            Action<string,  object> onDeserialized = ( json, cSharpObj ) => {                                
                Assert.Equal(serializedData, json);
                Assert.True(cSharpObj is Person);
            };
            var customDataSerializer = new SerializationMock(onDeserialized);

            Action<string,  object> onDeserializedMeta = ( json, cSharpObj ) => {                                
                Assert.Equal(serializedMetaData, json);
                Assert.True(cSharpObj is List<KeyValue>);
            };
            var customMetaSerializer = new SerializationMock(onDeserializedMeta);
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Sending two serializers
            var observable = factory.GetSerialized<Person, List<KeyValue>>(eventName, customDataSerializer, customMetaSerializer);

            observable.Subscribe(msg => {
				Assert.Equal(message.Name, msg.Data.Name);
                Assert.Equal(message.Age, msg.Data.Age);
                Assert.Equal(metaData, msg.MetaData);
			});
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, string>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, serializedData);
        }
    }
}
