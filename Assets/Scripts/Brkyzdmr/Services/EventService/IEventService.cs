namespace Brkyzdmr.Services.EventService
{
    public interface IEventService
    {
        T Get<T>() where T : IEvent, new();
    }
}