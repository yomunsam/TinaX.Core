#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinaX.Core.EventCenter
{
    public static class EventCenter
    {
        private static readonly Dictionary<string, EventCenterGroup> m_Groups = new Dictionary<string, EventCenterGroup>();
        private static readonly EventCenterGroup m_DefaultGroup = new EventCenterGroup();

        static EventCenter()
        {

        }


        /// <summary>
        /// Send Event(Message)
        /// </summary>
        /// <typeparam name="TSender">sender type</typeparam>
        /// <typeparam name="TArgs">args</typeparam>
        /// <param name="sender">sender object</param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <param name="group"></param>
        public static void Send<TSender, TArgs>(TSender sender, string eventName, TArgs args, string? group = null) where TSender: class
        {
            if (group == null)
            {
                m_DefaultGroup.Send<TSender, TArgs>(sender, eventName, args);
            }
            else
            {
                if(m_Groups.TryGetValue(eventName, out var groupObj))
                {
                    groupObj.Send<TSender, TArgs>(sender, eventName, args);
                }
            }
        }

        public static void Send(string eventName, object? args = null, string? group = null)
        {
            if (group == null)
                m_DefaultGroup.Send_ObjectArgs(eventName, args);
            else
            {
                if (m_Groups.TryGetValue(eventName, out var groupObj))
                {
                    groupObj.Send_ObjectArgs(eventName, args);
                }
            }
        }

        public static void Send<TArgs>(string eventName, TArgs args, string? group = null)
        {
            if(group == null)
                m_DefaultGroup.Send<TArgs>(eventName, args);
            else
            {
                if(m_Groups.TryGetValue(eventName, out var groupObj))
                {
                    groupObj.Send<TArgs>(eventName, args);
                }
            }
        }

        public static IEventSubscribe Subscribe(string eventName, EventHandlerEmpty callback, string? group = null)
        {
            if (group == null)
            {
                return m_DefaultGroup.Subscribe(eventName, callback);
            }
            else
            {
                if (!m_Groups.ContainsKey(group))
                    m_Groups[group] = new EventCenterGroup(group);
                return m_Groups[group].Subscribe(eventName, callback);
            }
        }


        public static IEventSubscribe Subscribe(string eventName, EventHandlerWithObjectArgs callback, string? group = null)
        {
            if (group == null)
            {
                return m_DefaultGroup.Subscribe(eventName, callback);
            }
            else
            {
                if (!m_Groups.ContainsKey(group))
                    m_Groups[group] = new EventCenterGroup(group);
                return m_Groups[group].Subscribe(eventName, callback);
            }
        }

        public static IEventSubscribe Subscribe(string eventName, EventHandlerWithObjectSenderAndArgs callback, string? group = null)
        {
            if (group == null)
            {
                return m_DefaultGroup.Subscribe(eventName, callback: callback);
            }
            else
            {
                if (!m_Groups.ContainsKey(group))
                    m_Groups[group] = new EventCenterGroup(group);
                return m_Groups[group].Subscribe(eventName, callback: callback);
            }
        }


        public static IEventSubscribe Subscribe<TArgs>(string eventName, EventHandler<TArgs> callback, string? group = null)
        {
            if (group == null)
            {
                return m_DefaultGroup.Subscribe(eventName, callback: callback);
            }
            else
            {
                if (!m_Groups.ContainsKey(group))
                    m_Groups[group] = new EventCenterGroup(group);
                return m_Groups[group].Subscribe(eventName, callback: callback);
            }
        }

        

        /// <summary>
        /// Subscibe Event
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IEventSubscribe Subscribe<TSender, TArgs>(string eventName, EventHandler<TSender, TArgs> callback, string? group = null) where TSender : class
        {
            if (group == null)
            {
                return m_DefaultGroup.Subscribe<TSender>(eventName, callback);
            }
            else
            {
                if (!m_Groups.ContainsKey(group))
                    m_Groups[group] = new EventCenterGroup(group);
                return m_Groups[group].Subscribe<TSender>(eventName, callback);
            }
        }


        public static void Unsubscribe(string eventName, Delegate eventHandler, string? group = null)
        {
            if (group == null)
            {
                m_DefaultGroup.Unsubscribe(eventName, eventHandler);
            }
            else
            {
                if (m_Groups.TryGetValue(eventName, out var groupObj))
                {
                    groupObj.Unsubscribe(eventName, eventHandler);
                }
            }
        }

    }



    public delegate void EventHandlerEmpty();
    public delegate void EventHandlerWithObjectArgs(object? args);
    public delegate void EventHandlerWithObjectSenderAndArgs(object? sender, object? args);

    public delegate void EventHandler<TArgs>(TArgs args);
    public delegate void EventHandler<TSender,TArgs>(TSender? sender, TArgs args) where TSender : class;

}
