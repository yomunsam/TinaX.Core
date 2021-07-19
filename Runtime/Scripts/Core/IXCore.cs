using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Container;

namespace TinaX
{
    public interface IXCore
    {
        bool IsRunning { get; }

        #region ServiceContainer
        IServiceContainer Services { get; }
        #endregion
    }
}
