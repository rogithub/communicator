using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Communicator
{
    public interface IEventSource<T>
    {
        IHandlerFactory<T> Handle { get; }
        IEventFactory<T> Send { get; }
        string ConnectionId { get; }
        Task<string> Connect();
    }

    public static class EventSourceFactory
    {
        public static IEventSource<T> Get<T>(string urlServer)
        {
            return new EventSource<T>(urlServer, new DefaultJsonSerializer());
        }

        public static IEventSource<T> Get<T>(string urlServer, IDataSerializer defaultSerializer)
        {
            return new EventSource<T>(urlServer, defaultSerializer);
        }
    }


    internal class EventSource<T> : IEventSource<T>
    {
        private HubConnection Connection { get; set; }
        public IHandlerFactory<T> Handle {get; private set;}
        public IEventFactory<T> Send {get; private set;}
        public IDataSerializer DataSerializer {get; private set;}

        public string ConnectionId {get; private set;}

        public EventSource(string urlServer, IDataSerializer serializer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);
            this.Handle = new HandlerFactory<T>(this.Connection, serializer);
            this.Send = new EventFactory<T>(this.Connection, serializer);
        }

        public async Task<string> Connect()
        {            
            //Connection.Closed += async (error) =>
            //{
            //    await Task.Delay(new Random().Next(0,5) * 1000);
            //    await Connection.StartAsync();
            //};

            await Connection.StartAsync();
            ConnectionId = await Connection.InvokeAsync<string>(EventNames.GetConnectionId);
            
            return ConnectionId;
        }
    }
}