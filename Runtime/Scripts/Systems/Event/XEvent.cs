using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Systems;

namespace TinaX
{
    public class XEvent
    {
        public const string DefaultGroup= "default";
        private struct EventRecord
        {
            public string name;
            public string group;
        }

        private static XEvent _instance;
        private static XEvent instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new XEvent();
                }
                return _instance;
            }
        }

        #region static
        public static IEventTicket Register(string EventName, Action<object> handler, string EventGroup = DefaultGroup)
        {
            return instance.RegisterEvent(EventName, handler, EventGroup);
        }

        public static void Call(string eventName, object param = null, string eventGroup = DefaultGroup)
        {
            instance.CallEvent(eventName, param, eventGroup);
        }

        public static void Remove(Action<object> handler)
        {
            instance.RemoveEvent(handler);
        }

        public static void Remove(Action<object> handler, string eventName, string eventGroup = DefaultGroup)
        {
            instance.RemoveEvent(handler, eventName, eventGroup);
        }

        #endregion

        /// <summary>
        /// EventGroup -> EventName -> Handlers
        /// </summary>
        private Dictionary<string, Dictionary<string, List<Action<object>>>> mDict_Handlers = new Dictionary<string, Dictionary<string, List<Action<object>>>>();
        private Dictionary<Action<object>, List<EventRecord>> mDict_Infos = new Dictionary<Action<object>, List<EventRecord>>();

        private XEvent()
        {
        }

        public IEventTicket RegisterEvent(string EventName, Action<object> handler, string EventGroup = DefaultGroup)
        {
            //是否已存在？
            if (mDict_Infos.ContainsKey(handler))
            {
                if (mDict_Infos[handler].Any(r => r.name == EventName && r.group == EventGroup))
                    return new EventTicket()
                    {
                        name = EventName,
                        group = EventGroup,
                        handler = handler
                    };
            }

            //登记
            if (!mDict_Handlers.ContainsKey(EventGroup))
                mDict_Handlers.Add(EventGroup, new Dictionary<string, List<Action<object>>>());
            if (!mDict_Handlers[EventGroup].ContainsKey(EventName))
                mDict_Handlers[EventGroup].Add(EventName, new List<Action<object>>());
            if (!mDict_Handlers[EventGroup][EventName].Contains(handler))
                mDict_Handlers[EventGroup][EventName].Add(handler);

            if (!mDict_Infos.ContainsKey(handler))
                mDict_Infos.Add(handler,new List<EventRecord>());
            mDict_Infos[handler].Add(new EventRecord { name = EventName, group = EventGroup });

            return new EventTicket() { name = EventName, group = EventGroup, handler = handler };
        }

        public void CallEvent(string eventName, object param = null, string eventGroup = DefaultGroup)
        {
            if(mDict_Handlers.TryGetValue(eventGroup,out var h_n))
            {
                if(h_n.TryGetValue(eventName,out var handlers))
                {
                    //foreach(var handler in handlers)
                    //{
                    //    handler.Invoke(param);
                    //}
                    for (var i = handlers.Count - 1; i >= 0; i--)
                    {
                        handlers[i]?.Invoke(param);
                    }
                }
            }
        }

        public void RemoveEvent(Action<object> handler)
        {
            if(mDict_Infos.TryGetValue(handler,out var records))
            {
                foreach(var item in records)
                {
                    if(mDict_Handlers.TryGetValue(item.group, out var h_n))
                    {
                        if(h_n.TryGetValue(item.name,out var handlers))
                        {
                            handlers.Remove(handler);
                            if(handlers.Count == 0)
                            {
                                h_n.Remove(item.name);
                            }
                        }
                        if(h_n.Count == 0)
                        {
                            mDict_Handlers.Remove(item.group);
                        }
                    }
                }
                mDict_Infos.Remove(handler);
            }
        }

        public void RemoveEvent(Action<object> handler, string eventName, string eventGroup = DefaultGroup)
        {
            if (mDict_Handlers.TryGetValue(eventGroup, out var h_n))
            {
                if (h_n.TryGetValue(eventName, out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        h_n.Remove(eventName);
                    }
                }
                if (h_n.Count == 0)
                {
                    mDict_Handlers.Remove(eventGroup);
                }
            }
            if(mDict_Infos.TryGetValue(handler,out var records))
            {
                foreach(var item in records)
                {
                    if(item.group == eventGroup && item.name == eventName)
                    {
                        records.Remove(item);
                        break;
                    }
                }
                if (records.Count == 0)
                    mDict_Infos.Remove(handler);
            }
        }

    }
}
