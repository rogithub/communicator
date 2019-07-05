using System.Xml;
using System;
using System.Xml.Serialization;

namespace Communicator
{
    public interface IPhrase
    {
	string DataCode { get; set; }
	string TextCode { get; set; }
	string Data { get; set; }
    }
    
    public class Phrase : IPhrase
    {
	private string _strData;
	private string _strDataCode;
	private string _strTextCode;
	

	public Phrase(string dataCode, string textCode, string data)
	{

	    if (string.IsNullOrWhiteSpace(dataCode))
		throw new ArgumentException("DataCode cannot be empty");

	    if (string.IsNullOrWhiteSpace(textCode))
		throw new ArgumentException("TextCode cannot be empty");

	    this.DataCode = dataCode;
	    this.TextCode = textCode;
	    this.Data = data;
	    
	}

	[XmlAttribute("DataCode")]
	public string DataCode
	{
	    get { return _strDataCode; }
	    set { _strDataCode = value; }
	    
	}

	[XmlAttribute("TextCode")]
	public string TextCode
	{
	    get { return _strTextCode; }
	    set { _strTextCode = value; }
	    
	}

	[XmlText()]
	public string Data
	{
	    get { return _strData; }
	    set { _strData = value; }
	    
	}	
    }
}
