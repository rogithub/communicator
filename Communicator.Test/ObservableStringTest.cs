using System;
using System.Collections.Generic;
using Xunit;
using Communicator.Core;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class ObservableStringTest
    {
        [Fact]
        public void GetString()
        {            
            var serializer = new JsonSerializer();           
            string eventName = "Chat";
            string message = "Hola";            
            var metaData = new List<KeyValue>();
            string serializedMetaData = serializer.Serialize(metaData);            
            
            ConnectionMock connection = new ConnectionMock();
            var factory = new ObservableFactory(connection, serializer);
            
            var observable = factory.GetString(eventName);

            observable.Subscribe(msg => {
				Assert.Equal(message, msg.Data);
                Assert.Equal(metaData, msg.MetaData);
			}); 
            

            var serverEvent = connection.ServerEvents[eventName] as Action<string, string>;
            Assert.NotNull(serverEvent);
            serverEvent(serializedMetaData, message);
        }        
    }
}
