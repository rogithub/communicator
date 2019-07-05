using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Communicator
{
    
    public class MetaData
    {
	public List<IValue> Values { get; set; }
	public List<IPhrase> Phrases { get; set; }

	public MetaData()
	{
	    this.Values = new List<IValue>();
	    this.Phrases = new List<IPhrase>();
	}

	public IEnumerable<IPhrase> GetPhrases(string dataCode)
	{
	    return (from f in Phrases where f.DataCode.Equals(dataCode) select f);
	}

	public IPhrase GetPhrase(string dataCode, string textCode)
	{
	    return (from f in Phrases where f.DataCode.Equals(dataCode) && f.TextCode.Equals(textCode) select f).FirstOrDefault();
	}

	public IEnumerable<string> GetPhraseStrings(string dataCode)
	{
	    return (from f in GetPhrases(dataCode) select f.Data);
	}

	public string GetPhraseString(string dataCode, string textCode, string defaultVal)
	{
	    IPhrase p = GetPhrase(dataCode, textCode);
	    if (p != null) return p.Data;

	    return defaultVal;
	}

	public string GetPhraseString(string dataCode, string textCode)
	{
	    return GetPhraseString(dataCode, textCode, string.Empty);
	}
	
	public string GetValueString(string dataCode, string defaultVal)
	{
	    var found = (from i in Values where !string.IsNullOrEmpty(i.DataCode) && i.DataCode.Equals(dataCode) select i).FirstOrDefault();
	    if (found != null && !string.IsNullOrEmpty(found.Data))
	    {
		return found.Data;
	    }

	    return defaultVal;
	}

	public string GetValueString(string dataCode)
	{
	    return GetValueString(dataCode, string.Empty);	    
	}

	public void SetValueString(string dataCode, string data)
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

	public void SetPhraseString(string dataCode, string textCode, string data)
	{	    
	    var found = (from i in Phrases where i.DataCode.Equals(dataCode) && i.TextCode.Equals(textCode) select i).FirstOrDefault();
	    if (found == null)
	    {
		found = new Phrase(dataCode, textCode, data);
		Phrases.Add(found);
	    }
	    else
	    {
		found.Data = data;		
	    }	    
	}
    }
}
