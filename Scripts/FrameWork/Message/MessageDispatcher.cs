using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class MessageDispatcher
    {
        public delegate void MessageHandle(Message msg);
        const int MaxCallDepth = 5;
        readonly Dictionary<int, MessageHandle> handlers = new Dictionary<int, MessageHandle>(32);
        int handleInCall;
        public void Subscribe(int msgType,MessageHandle msgHandle)
        {
            if (null != msgHandle)
            {
                MessageHandle rawList;
                handlers.TryGetValue(msgType, out rawList);
                handlers[msgType] = rawList + msgHandle;
            }
        }

        public void Unsubscribe(int msgType, MessageHandle msgHandle, bool keepEvent = false)
        {
            if (msgHandle != null)
            {
                MessageHandle rawList;
                if (handlers.TryGetValue(msgType, out rawList))
                {
                    var list = rawList - msgHandle;
                    if (list == null && !keepEvent)
                    {
                        handlers.Remove(msgType);
                    }
                    else
                    {
                        handlers[msgType] = list;
                    }
                }
            }
        }

        public void UnsubscribeAll(int msgType, bool keepEvent = false)
        {
            MessageHandle rawList;
            if (handlers.TryGetValue(msgType, out rawList))
            {
                if (keepEvent)
                {
                    handlers[msgType] = null;
                }
                else
                {
                    handlers.Remove(msgType);
                }
            }
        }

        public void UnsubscribeAndClearAllEvents()
        {
            handlers.Clear();
        }

        public void Publish(Message message)
        {
            if (handleInCall >= MaxCallDepth)
            {
#if UNITY_EDITOR
                throw new Exception("Max call depth reached");
#endif
            }
            int eventType = message.type();
            MessageHandle list;
            handlers.TryGetValue(eventType, out list);
            if (list != null)
            {
                handleInCall++;
                list(message);
                handleInCall--;
            }
        }
    }
}
