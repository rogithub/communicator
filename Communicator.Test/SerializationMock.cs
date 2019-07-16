using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using Communicator.Core;

namespace Communicator.Test
{
    public class SerializationMock : IDataSerializer
    {
        private JsonSerializer Serializer { get; set; }
        private Action<string, object> OnDeserialized { get; set; }
        private Action<object, string> OnSerialized { get; set; }

        public SerializationMock(Action<string, object> onDeserialized, Action<object, string> onSerialized)
        {
            Serializer = new JsonSerializer();
            this.OnDeserialized = onDeserialized;
            this.OnSerialized = onSerialized;
        }

        public SerializationMock(Action<string, object> onDeserialized) : this(onDeserialized, (t, json) => {})
        {
        }

        public SerializationMock(Action<object, string> onSerialized) : this((json, t) => {}, onSerialized)
        {
        }

        public T Deserialize<T>(string json)
		{
			T result = Serializer.Deserialize<T>(json);
            OnDeserialized(json, result);
            return result;
		}

		public string Serialize<T>(T cSharpObj)
		{
			string json = Serializer.Serialize<T>(cSharpObj);
            OnSerialized(cSharpObj, json);
            return json;
		}
    }
}
