
using Communicator;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Chat
{

    public class ActiveConnections
    {
        private Dictionary<string, string> Users = new Dictionary<string, string>();

		public void AddUser(MetaData md)
		{
			string user = md.Get("user");
			string id = md.Get("id");
			AddUser(user, id);
		}
		public void AddUser(string user, string id)
		{	
			if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(id))
			{
				if (!Users.ContainsKey(id)) {
					Users.Add(id, user);					
				}

				Users[id] = user;
			}
		}

        public string FindId(string user)
		{	
			return Users.FirstOrDefault(it => it.Value == user ).Key;
		}

        public void Remove(string id)
        {
            Users.Remove(id);
        }

		public void Print()
        {
            foreach(var it in Users)
			{
				Console.WriteLine($"{it.Key}: {it.Value}");
			}
        }
    } 
}