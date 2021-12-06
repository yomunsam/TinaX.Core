using System;
using TinaX.Core.Utils;

namespace TinaX
{
    public static class XEventExtend
    {
        public static DisposableGroup RegisterEvent(this DisposableGroup dg, string EventName, Action<object> Handler, string EventGroup = XEvent.DefaultGroup)
            => dg.Register(XEvent.Register(EventName, Handler, EventGroup));
    }
}
