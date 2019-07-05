
using System;
using System.Xml;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MVC
{

	public class CommunicatorHub : Hub
	{
		public async Task<Guid> SendXml(string eventName, string user, XmlDocument data)
		{
			await Clients.Others.SendAsync(eventName, user, data);
			return Guid.NewGuid();
		}
		public async Task<Guid> SendString(string eventName, string user, string  data)
		{
			await Clients.Others.SendAsync(eventName, user, data);
			return Guid.NewGuid();
		}

		public async Task<Guid> SendBinary(string eventName, string user, byte[] data)
		{
			await Clients.Others.SendAsync(eventName, user, data);
			return Guid.NewGuid();
		}
	}
}