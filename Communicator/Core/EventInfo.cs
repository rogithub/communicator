using System;
using System.Runtime.Serialization;

namespace Communicator.Core
{
    [DataContract]
    public class EventInfo
    {   
        public EventInfo(string eventName, string[] to, string[] groups)
        {
            if (string.IsNullOrWhiteSpace(eventName))
			throw new ArgumentException("EventName cannot be empty");

            EventName = eventName;
            To = to;
            Groups = groups;
        }

        public EventInfo(string eventName): this(eventName, Array.Empty<string>(), Array.Empty<string>())
        {
            
        }

        public EventInfo(string eventName, string to, string group) : 
        this(
            eventName, 
            string.IsNullOrWhiteSpace(to) ? Array.Empty<string>() : new string[] { to }, 
            string.IsNullOrWhiteSpace(group) ? Array.Empty<string>() : new string[] { group })
        {   

        }

        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public string[] To { get; set; }
        [DataMember]
        public string[] Groups { get; set; }
    }

}