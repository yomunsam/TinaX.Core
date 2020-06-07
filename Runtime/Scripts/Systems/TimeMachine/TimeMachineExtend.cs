using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Utils;

namespace TinaX
{
    public static class TimeMachineExtend
    {
        public static DisposableGroup RegisterUpdate(this DisposableGroup dg, Action updateAction, int order = 0)
            => dg.Register(
                TimeMachine.RegisterUpdate(updateAction, order));

        public static DisposableGroup RegisterLateUpdate(this DisposableGroup dg, Action lateupdateAction, int order = 0)
            => dg.Register(
                TimeMachine.RegisterLateUpdate(lateupdateAction, order));

        public static DisposableGroup RegisterFixedUpdate(this DisposableGroup dg, Action fixedupdateAction, int order = 0)
            => dg.Register(
                TimeMachine.RegisterFixedUpdate(fixedupdateAction, order));
    }
}
