
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communicator.Core;
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

		public async Task<Guid> SendBinaryToGeneric(EventInfo info, string metaData, byte[] data)
		{
			Task task = (info.To == null || info.To.Length == 0) ?
			Clients.Others.SendAsync(info.EventName, metaData, data):
			Clients.Clients(info.To).SendAsync(info.EventName, metaData, data);
			await task;
			
			return Guid.NewGuid();
		}
		
		
		public async Task<Guid> SendStringToGeneric(EventInfo info, string metaData, string data)
		{			
			Task task = (info.To == null || info.To.Length == 0) ?
			Clients.Others.SendAsync(info.EventName, metaData, data):
			Clients.Clients(info.To).SendAsync(info.EventName, metaData, data);
			
			await task;
			return Guid.NewGuid();
		}

		public async Task<Guid> SendBinaryTo(EventInfo info, List<MetaData> metaData, byte[] data)
		{
			Task task = (info.To == null || info.To.Length == 0) ?
			Clients.Others.SendAsync(info.EventName, metaData, data):
			Clients.Clients(info.To).SendAsync(info.EventName, metaData, data);
			await task;
			
			return Guid.NewGuid();
		}
		
		
		public async Task<Guid> SendStringTo(EventInfo info, List<MetaData> metaData, string data)
		{			
			Task task = (info.To == null || info.To.Length == 0) ?
			Clients.Others.SendAsync(info.EventName, metaData, data):
			Clients.Clients(info.To).SendAsync(info.EventName, metaData, data);
			
			await task;
			return Guid.NewGuid();
		}
	}
}
