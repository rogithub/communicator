using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System;

namespace Communicator.Core
{
    [DataContract]
    public struct MetaData
    {		
		public MetaData(string key) : this(key, string.Empty)
		{
			
		}

		public MetaData(string key, string value)
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
    
    public static class MetaDataHelpers
    {	
		public static bool Exists(this IEnumerable<MetaData> list, string key)
		{			
			return list.Where(i => i.Key.Equals(key)).Count() > 0;						
		}

		public static string Get(this IEnumerable<MetaData> list, string key, string defaultVal)
		{		
			if (!Exists(list, key)) return defaultVal;
			return (from i in list where  i.Key.Equals(key) select i).First().Value;
		}

		public static string Get(this IEnumerable<MetaData> list, string key)
		{		
			return Get(list, key, string.Empty);
		}

		public static void Set(this List<MetaData> list, string key, string value)
		{		
			list.RemoveAll(i => i.Key.Equals(key));
			var newItem = new MetaData(key, value);
			list.Add(newItem);
		}
    }
}
