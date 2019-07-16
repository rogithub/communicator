using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Communicator
{    
    internal class HubConnectionProxy : IHubConnection
    {
        private HubConnection HubConnection { get; set; }
        public event Func<Exception, Task> Closed
        {
            add { HubConnection.Closed += value; }
            remove { HubConnection.Closed -= value; }
        }
        public HubConnectionProxy(HubConnection connection)
        {
            this.HubConnection = connection;            
        }

        public Task StartAsync()
        {
            return HubConnection.StartAsync();
        }

        public IDisposable On<M, D>(string eventName, Action<M, D> action)
        {
            return HubConnection.On<M, D>(eventName, action);            
        }

        public IDisposable On<T>(string eventName, Action<T> action)
        {
            return HubConnection.On<T>(eventName, action);            
        }

        public Task<Guid> InvokeAsync(string eventName, object arg1, object arg2, object arg3, object arg4)
        {
            return HubConnection.InvokeAsync<Guid>(eventName, arg1, arg2, arg3, arg4);
        }

        public Task<T> InvokeAsync<T>(string eventName, Action<T> action)
        {
            return HubConnection.InvokeAsync<T>(eventName, action);
        }

        public Task<T> InvokeAsync<T>(string eventName)
        {
            return HubConnection.InvokeAsync<T>(eventName);
        }
    }

}