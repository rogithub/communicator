using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System;

namespace Communicator.Core
{
    [DataContract]	
    public struct KeyValue
    {		
		public KeyValue(string key) : this(key, string.Empty)
		{
			
		}

		public KeyValue(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
			throw new ArgumentException("Key cannot be empty");

			this.Key = key;
			this.Value = value;
		}

		[DataMember]
		public string Key { get; set; }

		[DataMember]  
		public string Value { get; set; }
    }    
    
    public static class KeyValueListHelpers
    {	
		public static bool Exists(this IEnumerable<KeyValue> list, string key)
		{			
			return list.Where(i => i.Key.Equals(key)).Count() > 0;						
		}

		public static string Get(this IEnumerable<KeyValue> list, string key, string defaultVal)
		{		
			if (!Exists(list, key)) return defaultVal;
			return (from i in list where  i.Key.Equals(key) select i).First().Value;
		}

		public static string Get(this IEnumerable<KeyValue> list, string key)
		{		
			return Get(list, key, string.Empty);
		}

		public static void Set(this List<KeyValue> list, string key, string value)
		{		
			list.RemoveAll(i => i.Key.Equals(key));
			var newItem = new KeyValue(key, value);
			list.Add(newItem);
		}
    }
}