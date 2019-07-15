using System;
using System.Threading;
using System.Threading.Tasks;

namespace Communicator.Test
{
    public class ConnectionMock : IHubConnection
    {
        public event Func<Exception, Task> Closed;        
        public Func<string, object, object, object, object, Guid> InvokeAction;
        public ConnectionMock(Func<string, object, object, object, object, Guid> invokeAction)
        {
            this.InvokeAction = invokeAction;
        }

        public void Close()
        {
            var ex = new NotImplementedException();
            Closed.Invoke(ex);
        }
        

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public IDisposable On<M, D>(string eventName, Action<M, D> action)
        {
            throw new NotImplementedException();
        }
                
        public IDisposable On<T>(string eventName, Action<T> action)
        {            
            throw new NotImplementedException();
        }

        public Task<T> InvokeAsync<T>(string eventName)
        {
            throw new NotImplementedException();
        }
        public Task<Guid> InvokeAsync(string eventName, object arg1, object arg2, object arg3, object arg4, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(InvokeAction(eventName, arg1, arg2, arg3, arg4));
        }

        public Task<T> InvokeAsync<T>(string eventName, Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}
