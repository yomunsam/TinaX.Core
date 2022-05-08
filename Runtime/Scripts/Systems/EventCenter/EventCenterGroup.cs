#nullable enable
using System;
using System.Collections.Generic;

namespace TinaX.Core.EventCenter
{
    internal class EventCenterGroup
    {
        private readonly string? m_Name;
        private readonly Dictionary<string, List<EventSubscribe>> m_Subscribes = new Dictionary<string, List<EventSubscribe>>();

        public EventCenterGroup(string? name = null)
        {
            this.m_Name = name;
        }

        public string? Name => m_Name;

        public EventSubscribe Subscribe(string eventName, Delegate callback)
        {
            if (!m_Subscribes.ContainsKey(eventName))
                m_Subscribes[eventName] = new List<EventSubscribe>();

            var subscribe = new EventSubscribe(this, eventName, callback);
            m_Subscribes[eventName].Add(subscribe);
            return subscribe;
        }

        public EventSubscribe Subscribe<TSender>(string eventName, Delegate callback) where TSender : class
        {
            if (!m_Subscribes.ContainsKey(eventName))
                m_Subscribes[eventName] = new List<EventSubscribe>();

            var subscribe = new EventSubscribe(this, eventName, callback);
            m_Subscribes[eventName].Add(subscribe);
            return subscribe;
        }

        public void Send<TSender, TArgs>(TSender? sender, string eventName, TArgs args) where TSender : class
        {
            if(m_Subscribes.TryGetValue(eventName, out var subscribes))
            {
                foreach (var item in subscribes)
                    item.Invoke<TSender, TArgs>(sender, args);
            }
        }

        public void Send<TArgs>(string eventName, TArgs args)
        {
            if(m_Subscribes.TryGetValue(eventName,out var subscribes))
            {
                foreach (var item in subscribes)
                    item.Invoke(args);
            }
        }

        public void Send_ObjectArgs(string eventName, object? args)
        {
            if (m_Subscribes.TryGetValue(eventName, out var subscribes))
            {
                foreach (var item in subscribes)
                    item.InvokeObjectArgs(args);
            }
        }

        public void Unsubscribe(in EventSubscribe subscribe)
        {
            if(m_Subscribes.TryGetValue(subscribe.EventName, out var subscribeList))
            {
                subscribeList.Remove(subscribe);
                if (subscribeList.Count < 1)
                {
                    m_Subscribes.Remove(subscribe.EventName);
                }
            }
        }

        public void Unsubscribe(in string eventName, in Delegate eventHandler)
        {
            if (m_Subscribes.TryGetValue(eventName, out var subscribeList))
            {
                for(int i = subscribeList.Count - 1; i >= 0; i--)
                {
                    var item = subscribeList[i];
                    if(item.IsMatchCallback(in eventHandler))
                    {
                        subscribeList.RemoveAt(i);
                    }
                }
                if(subscribeList.Count < 1)
                    m_Subscribes.Remove(eventName);
            }
        }
    }
}
