using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System;

namespace Communicator
{
    [DataContract]
    public class Value
    {
		public Value()
		{
			
		}
		
		public Value(string dataCode) : this(dataCode, string.Empty)
		{
			
		}

		public Value(string dataCode, string data)
		{
			if (string.IsNullOrWhiteSpace(dataCode))
			throw new ArgumentException("DataCode cannot be empty");

			this.DataCode = dataCode;
			this.Data = data;
		}

		[DataMember]
		public string DataCode { get; set; }

		[DataMember]  
		public string Data { get; set; }
    }    
    
    public static class ValueHelpers
    {		
		public static string Get(this IEnumerable<Value> list, string dataCode, string defaultVal)
		{
			var found = (from i in list where !string.IsNullOrEmpty(i.DataCode) && i.DataCode.Equals(dataCode) select i).FirstOrDefault();
			if (found != null && !string.IsNullOrEmpty(found.Data))
			{
				return found.Data;
			}

			return defaultVal;
		}

		public static string Get(this IEnumerable<Value> list, string dataCode)
		{
			return Get(list, dataCode, string.Empty);
		}

		public static void Set(this List<Value> list, string dataCode, string data)
		{	    
			var found = (from i in list where !string.IsNullOrEmpty(i.DataCode) && i.DataCode.Equals(dataCode) select i).FirstOrDefault();
			if (found == null)
			{
				found = new Value(dataCode, data);
				list.Add(found);	
			}
			else
			{
				found.Data = data;	
			}	    
		}
    }
}
