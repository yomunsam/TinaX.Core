#nullable enable
using System;

namespace TinaX.Core.EventCenter
{
    /// <summary>
    /// 事件订阅 内部实现类
    /// </summary>
    internal class EventSubscribe : IEventSubscribe
    {
        private readonly EventCenterGroup m_Group;
        private readonly string m_EventName;
        private readonly Delegate m_Callback;

        public EventSubscribe(EventCenterGroup group,string eventName, Delegate callback)
        {
            this.m_Group = group;
            this.m_EventName = eventName;
            this.m_Callback = callback;
        }

        public string EventName => m_EventName;


        public void Invoke<TSender, TArgs>(TSender? sender, TArgs args) where TSender : class
        {
            if (m_Callback is EventHandler<TSender, TArgs> eventHandler_sender_args)
            {
                eventHandler_sender_args.Invoke(sender, args);
                return;
            }

            if(m_Callback is EventHandler<TArgs> eventHandler_args)
            {
                eventHandler_args.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerEmpty eventHandlerEmpty)
            {
                eventHandlerEmpty.Invoke();
                return;
            }

            if (m_Callback is EventHandlerWithObjectArgs eventHandlerWithObjectArgs)
            {
                eventHandlerWithObjectArgs.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerWithObjectSenderAndArgs eventHandlerWithObjectSenderAndArgs)
            {
                eventHandlerWithObjectSenderAndArgs.Invoke(sender, args);
                return;
            }

        }

        public void Invoke<TArgs>(TArgs args)
        {
            if (m_Callback is EventHandler<TArgs> eventHandler_args)
            {
                eventHandler_args.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerEmpty eventHandlerEmpty)
            {
                eventHandlerEmpty.Invoke();
                return;
            }

            if (m_Callback is EventHandlerWithObjectArgs eventHandlerWithObjectArgs)
            {
                eventHandlerWithObjectArgs.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerWithObjectSenderAndArgs eventHandlerWithObjectSenderAndArgs)
            {
                eventHandlerWithObjectSenderAndArgs.Invoke(null, args);
                return;
            }
        }

        public void InvokeObjectArgs(object? args)
        {
            if (m_Callback is EventHandler<object?> eventHandler_args)
            {
                eventHandler_args.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerEmpty eventHandlerEmpty)
            {
                eventHandlerEmpty.Invoke();
                return;
            }

            if (m_Callback is EventHandlerWithObjectArgs eventHandlerWithObjectArgs)
            {
                eventHandlerWithObjectArgs.Invoke(args);
                return;
            }

            if (m_Callback is EventHandlerWithObjectSenderAndArgs eventHandlerWithObjectSenderAndArgs)
            {
                eventHandlerWithObjectSenderAndArgs.Invoke(null, args);
                return;
            }
        }

        public void Unsubscribe()
        {
            m_Group.Unsubscribe(this);
        }



        public bool IsMatchCallback(in Delegate callback)
        {
            return m_Callback == callback;
        }

    }
}
