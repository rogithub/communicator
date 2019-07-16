using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Communicator.Test")]
namespace Communicator
{        
    internal interface IHubConnection
    {
        event Func<Exception, Task> Closed;
        Task StartAsync();
        IDisposable On<M, D>(string eventName, Action<M, D> action);
        IDisposable On<T>(string eventName, Action<T> action);
        Task<Guid> InvokeAsync(string eventName, object arg1, object arg2, object arg3, object arg4);
        Task<T> InvokeAsync<T>(string eventName, Action<T> action);
        Task<T> InvokeAsync<T>(string eventName);
    }    
}