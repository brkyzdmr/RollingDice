using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brkyzdmr.Services.EventService
{
    public abstract class GameEvent : IEvent
    {
        private Action _callback;
        private Dictionary<GameObject, Action> _subscriberHandlerDictionary = new Dictionary<GameObject, Action>();

        public virtual void AddListener(Action handler, GameObject subscriber = null)
        {
            if (subscriber == null) _callback += handler;
            else
            {
                if (!_subscriberHandlerDictionary.ContainsKey(subscriber))
                {
                    _subscriberHandlerDictionary.Add(subscriber, handler);
                    _callback += handler;
                }
            }
        }

        public void RemoveListener(Action handler, GameObject subscriber = null)
        {
            if (subscriber == null) _callback -= handler;
            else
            {
                if (_subscriberHandlerDictionary.ContainsKey(subscriber))
                {
                    _subscriberHandlerDictionary.Remove(subscriber);
                    _callback -= handler;
                }
            }
        }

        public void Execute()
        {
            var subscribersToRemove = _subscriberHandlerDictionary.Where(x => x.Key == null || !x.Key.activeInHierarchy).ToArray();
            foreach (var subscriber in subscribersToRemove)
            {
                _callback -= _subscriberHandlerDictionary[subscriber.Key];
                _subscriberHandlerDictionary.Remove(subscriber.Key);
            }

            _callback?.Invoke();
        }
    }
    
    public abstract class GameEvent<T1> : GameEvent
    {
        private Action<T1> _callback;

        public void AddListener(Action<T1> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T1> handler)
        {
            _callback -= handler;
        }

        public void Execute(T1 arg1)
        {
            _callback?.Invoke(arg1);
        }
    }
    
    public abstract class GameEvent<T1, T2> : GameEvent
    {
        private Action<T1, T2> _callback;

        public void AddListener(Action<T1, T2> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T1, T2> handler)
        {
            _callback -= handler;
        }

        public void Execute(T1 arg1, T2 arg2)
        {
            _callback?.Invoke(arg1, arg2);
        }
    }

    public abstract class GameEvent<T1, T2, T3> : GameEvent
    {
        private Action<T1, T2, T3> _callback;

        public void AddListener(Action<T1, T2, T3> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T1, T2, T3> handler)
        {
            _callback -= handler;
        }

        public void Execute(T1 arg1, T2 arg2, T3 arg3)
        {
            _callback?.Invoke(arg1, arg2, arg3);
        }
    }
}