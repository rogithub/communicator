namespace Communicator
{   
    public static class EventSourceFactory
    {
        public static IEventSource Get(string urlServer)
        {
            return new EventSource(urlServer);
        }
    }    
}