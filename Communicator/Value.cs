
using System;
using System.Runtime.Serialization;

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
}
