using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Utils;

namespace TinaX
{
    public static class XEventExtend
    {
        public static DisposableGroup RegisterEvent(this DisposableGroup dg, string EventName, Action<object> Handler, string EventGroup = XEvent.DefaultGroup)
            => dg.Register(XEvent.Register(EventName, Handler, EventGroup));
    }
}
