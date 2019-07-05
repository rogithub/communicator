
using System;
using System.Xml;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MVC
{

	public class CommunicatorHub : Hub
	{
		public async Task<Guid> SendXml(string eventName, string metaData, XmlDocument data)
		{
			await Clients.Others.SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}
		public async Task<Guid> SendString(string eventName, string metaData, string  data)
		{
			await Clients.Others.SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}

		public async Task<Guid> SendBinary(string eventName, string metaData, byte[] data)
		{
			await Clients.Others.SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}
	}
}
