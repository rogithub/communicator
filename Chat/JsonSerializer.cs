using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Communicator.Core;

namespace Chat
{
    internal class JsonSerializer : IDataSerializer
    {
		public T Deserialize<T>(string json)
		{
			
			using(MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{  
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));  
				T cSharpObj = (T)ser.ReadObject(ms); 
				ms.Close();
				
				return cSharpObj;				
			}			
		}

		public string Serialize<T>(T cSharpObj)
		{
			using (MemoryStream ms = new MemoryStream())
			{   
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));  
				ser.WriteObject(ms, cSharpObj);  
				byte[] json = ms.ToArray();  
				ms.Close();

				return Encoding.UTF8.GetString(json, 0, json.Length);
			}   
		}
		
    }    
}