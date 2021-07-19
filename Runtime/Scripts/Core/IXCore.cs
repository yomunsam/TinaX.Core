using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Module;

namespace TinaX
{
    public interface IXCore
    {
        #region Informations
        string FrameworkVersionName { get; }
        #endregion

        #region Status
        bool IsRunning { get; }

        #endregion

        #region ServiceContainer
        IServiceContainer Services { get; }

        #endregion

        #region Modules
        IXCore AddModule(IModuleProvider moduleProvider);
        UniTask RunAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
