using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class ObservableBinaryTest
    {
        [Fact]
        public void GetBinary()
        {            
            var serializer = new JsonSerializer();           
            string eventName = "File";
            byte[] message = new byte[] { 1, 2, 3};            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);            
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Not sending Generic MetaData
            var observable = factory.GetBinary(eventName);

            observable.Subscribe(msg => {
				Assert.Equal(message, msg.Data);
                Assert.Equal(metaData, msg.MetaData);
			});
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, byte[]>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, message);
        }        

        [Fact]
        public void GetBinaryGenericMetaData()
        {            
            var serializer = new JsonSerializer();           
            string eventName = "File";
            byte[] message = new byte[] { 1, 2, 3};            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);            
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            // Sending generic T
            var observable = factory.GetBinary<List<KeyValue>>(eventName, serializer);

            observable.Subscribe(msg => {
				Assert.Equal(message, msg.Data);
                Assert.Equal(metaData, msg.MetaData);
			});
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, byte[]>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, message);
        }        
    }
}
