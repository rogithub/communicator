using System.Xml;
using System;
using System.Xml.Serialization;

namespace Communicator
{
    public interface IValue
    {
	string DataCode { get; set; }
	string Data { get; set; }
    }

    public class Value: IValue
    {

	private string _strDataCode;
	private string _strData;
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

	[XmlAttribute()]
	public string DataCode
	{
	    get { return _strDataCode; }
	    set { _strDataCode = value; }
	    
	}

	[XmlText()]
	public string Data
	{
	    get { return _strData; }
	    set { _strData = value; }
	    
	}	
    }
}