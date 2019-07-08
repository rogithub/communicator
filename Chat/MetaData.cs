using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Chat
{

    [DataContract]
    public class MetaData
    {
		[DataMember]  
		public List<Value> Values { get; set; }

		public MetaData()
		{
			this.Values = new List<Value>();
		}

		public string Get(string dataCode, string defaultVal)
		{
			var found = (from i in Values where !string.IsNullOrEmpty(i.DataCode) && i.DataCode.Equals(dataCode) select i).FirstOrDefault();
			if (found != null && !string.IsNullOrEmpty(found.Data))
			{
				return found.Data;
			}

			return defaultVal;
		}

		public string Get(string dataCode)
		{
			return Get(dataCode, string.Empty);	    
		}

		public void Set(string dataCode, string data)
		{	    
			var found = (from i in Values where !string.IsNullOrEmpty(i.DataCode) && i.DataCode.Equals(dataCode) select i).FirstOrDefault();
			if (found == null)
			{
				found = new Value(dataCode, data);
				Values.Add(found);	
			}
			else
			{
				found.Data = data;	
			}	    
		}
    }
}
