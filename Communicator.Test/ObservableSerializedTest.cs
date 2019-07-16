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
            var serializer = new JsonSerializer();           
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Not sending Default MetaData
            var observable = factory.GetSerialized<Person>(eventName, serializer);

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
            var serializer = new JsonSerializer();           
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Sending generic MetaData
            var observable = factory.GetSerialized<Person, List<KeyValue>>(eventName, serializer);

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
        public void GetSerializedDataAndMeta()
        {            
            var serializer = new JsonSerializer();           
            string eventName = "Person";
            Person message = new Person() { Name = "Alice", Age = 27 };            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);
            string serializedData = serializer.Serialize(message);
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Sending two serializers
            var observable = factory.GetSerialized<Person, List<KeyValue>>(eventName, serializer, serializer);

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
