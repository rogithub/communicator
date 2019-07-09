
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

		public async Task<Guid> SendBinaryTo(string[] ids, string eventName, string metaData, byte[] data)
		{
			Task task = (ids == null || ids.Length == 0) ?
			Clients.Others.SendAsync(eventName, metaData, data):
			Clients.Clients(ids).SendAsync(eventName, metaData, data);
			await task;
			
			return Guid.NewGuid();
		}
		
		
		public async Task<Guid> SendStringTo(string[] ids, string eventName, string metaData, string data)
		{			
			Task task = (ids == null || ids.Length == 0) ?
			Clients.Others.SendAsync(eventName, metaData, data):
			Clients.Clients(ids).SendAsync(eventName, metaData, data);			
			
			await task;
			return Guid.NewGuid();
		}
	}
}
