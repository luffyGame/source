using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class EventBus
    {
        public delegate void EventHandler<T>(T eventData);
        const int MaxCallDepth = 5;

        readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>(32);
        int _eventsInCall;
        public void Subscribe<T>(EventHandler<T> eventAction)
        {
            if(null!=eventAction)
            {
                var eventType = typeof(T);
                Delegate rawList;
                _events.TryGetValue(eventType, out rawList);
                _events[eventType] = (rawList as EventHandler<T>) + eventAction;
            }
        }

        public void Unsubscribe<T>(EventHandler<T> eventAction, bool keepEvent = false)
        {
            if (eventAction != null)
            {
                var eventType = typeof(T);
                Delegate rawList;
                if (_events.TryGetValue(eventType, out rawList))
                {
                    var list = (rawList as EventHandler<T>) - eventAction;
                    if (list == null && !keepEvent)
                    {
                        _events.Remove(eventType);
                    }
                    else
                    {
                        _events[eventType] = list;
                    }
                }
            }
        }

        public void UnsubscribeAll<T>(bool keepEvent = false)
        {
            var eventType = typeof(T);
            Delegate rawList;
            if (_events.TryGetValue(eventType, out rawList))
            {
                if (keepEvent)
                {
                    _events[eventType] = null;
                }
                else
                {
                    _events.Remove(eventType);
                }
            }
        }

        public void UnsubscribeAndClearAllEvents()
        {
            _events.Clear();
        }

        public void Publish<T>(T eventMessage)
        {
            if (_eventsInCall >= MaxCallDepth)
            {
#if UNITY_EDITOR
                throw new Exception("Max call depth reached");
#endif
            }
            var eventType = typeof(T);
            Delegate rawList;
            _events.TryGetValue(eventType, out rawList);
            var list = rawList as EventHandler<T>;
            if (list != null)
            {
                _eventsInCall++;
                list(eventMessage);
                _eventsInCall--;
            }
        }
    }
}
