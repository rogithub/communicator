
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MVC
{
	public class CommunicatorHub : Hub
	{
		public string GetConnectionId()
		{			
			return Context.ConnectionId;
		}
		
		public override async Task OnConnectedAsync()
		{			
			await base.OnConnectedAsync();
			await Clients.Others.SendAsync("Communicator.OnConnected", Context.ConnectionId);
		}

		public override async Task OnDisconnectedAsync(Exception ex)
		{			
			await base.OnDisconnectedAsync(ex);
			await Clients.Others.SendAsync("Communicator.OnDisconnected", Context.ConnectionId);
		}		

		public async Task<Guid> SendBinary(string eventName, string metaData, byte[] data)
		{
			await Clients.Others.SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}

		public async Task<Guid> SendString(string eventName, string metaData, string data)
		{
			await Clients.Others.SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}
		public async Task<Guid> SendStringTo(string connectionId, string eventName, string metaData, string data)
		{
			await Clients.Client(connectionId).SendAsync(eventName, metaData, data);
			return Guid.NewGuid();
		}
	}
}
