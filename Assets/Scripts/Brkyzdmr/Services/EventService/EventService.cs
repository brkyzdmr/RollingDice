namespace Brkyzdmr.Services.EventService
{
    public class EventService : Service, IEventService
    {
        private readonly EventHub _eventHub = new EventHub();

        public T Get<T>() where T : IEvent, new()
        {
            return _eventHub.Get<T>();
        }
    }
}