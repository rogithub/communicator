using System.Threading.Tasks;

namespace Communicator
{    
    internal class EventSource : IEventSource
    {
        private IHubConnection Connection { get; set; }        

        public EventSource(string urlServer)
        {
            this.Connection = ConnectionBuilder.Build(urlServer);            
        }

        public Task Connect()
        {
            return Connection.StartAsync();
        }        

        public IObservableFactory GetObservablesFactory()
        {
            return new ObservableFactory(Connection, new JsonSerializer());
        }

        public IEventSender GetEventSender()
        {
            return new EventSender(Connection, new JsonSerializer());
        }

        public Task<string> GetConnectionId()
        {
            return Connection.InvokeAsync<string>(EventNames.GetConnectionId); 
        }
    }
}