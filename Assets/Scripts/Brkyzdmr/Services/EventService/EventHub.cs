using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.EventService
{
    public class EventHub
    {
        private Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        public T Get<T>() where T : IEvent,
            new()
        {
            Type eType = typeof(T);
            IEvent eventToReturn;
            if (_events.TryGetValue(eType, out eventToReturn))
                return (T)eventToReturn;

            eventToReturn = (GameEvent)Activator.CreateInstance(eType);
            _events.Add(eType, eventToReturn);
            return (T)eventToReturn;
        }
    }
}