using System;

namespace TinaX.Systems
{
    public interface IEventTicket
    {
        void Unregister();
    }

    public struct EventTicket : IEventTicket
    {
        public Action<object> handler;
        public string group;
        public string name;

        public void Unregister()
        {
            XEvent.Remove(handler, name, group);
        }
    }
}
