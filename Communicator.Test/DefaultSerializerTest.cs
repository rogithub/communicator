using System.Collections.Generic;
using Xunit;
using Communicator.Core;

namespace Communicator.Test
{
    public class DefaultSerializerTest
    {
        [Fact]
        public void ShouldSerializePerson()
        {
            var serializer = new JsonSerializer();            
            var person = new Person() { Name = "Jhon Doe", Age = 55 };
            var strMsg = "{\"Age\":55,\"Name\":\"Jhon Doe\"}";

             Assert.Equal(strMsg, serializer.Serialize(person));
             Assert.Equal(person.Name, serializer.Deserialize<Person>(strMsg).Name);
             Assert.Equal(person.Age, serializer.Deserialize<Person>(strMsg).Age);
        }

        [Fact]
        public void ShouldSerializeMetadata()
        {
            var serializer = new JsonSerializer();
            var metadata = new List<KeyValue>();
            string name = "Rodrigo";
            string age = "35";
            metadata.Set("Age", age);
            metadata.Set("Name", name);

            var strMsg = "[{\"Key\":\"Age\",\"Value\":\"35\"},{\"Key\":\"Name\",\"Value\":\"Rodrigo\"}]";

             Assert.Equal(strMsg, serializer.Serialize(metadata));
             Assert.Equal(name, serializer.Deserialize<List<KeyValue>>(strMsg).Get("Name"));
             Assert.Equal(age, serializer.Deserialize<List<KeyValue>>(strMsg).Get("Age"));
        }
    }
}
