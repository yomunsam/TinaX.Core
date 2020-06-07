using System;

namespace TinaX.Systems
{
    public interface IEventTicket : IDisposable
    {
        void Unregister();
    }

    public struct EventTicket : IEventTicket
    {
        public Action<object> handler;
        public string group;
        public string name;

        public void Dispose()
        {
            this.Unregister();
        }

        public void Unregister()
        {
            XEvent.Remove(handler, name, group);
        }

    }
}
