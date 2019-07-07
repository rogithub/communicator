
using System;
using System.Runtime.Serialization;

namespace Communicator
{

	[DataContract]
    public class Value
    {

		private string _strDataCode;
		private string _strData;

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
		public string DataCode
		{
			get { return _strDataCode; }
			set { _strDataCode = value; }	    
		}

		[DataMember]  
		public string Data
		{
			get { return _strData; }
			set { _strData = value; }
			
		}	
    }
}
