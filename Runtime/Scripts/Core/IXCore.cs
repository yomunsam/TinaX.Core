/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Core.Activator;
using TinaX.Core.Behaviours;
using TinaX.Module;

namespace TinaX
{
    public interface IXCore
    {
        #region Build
        bool ReflectIXBootstrap { get; set; }
        #endregion

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

        #endregion

        #region AppDomains
        object CreateInstance(Type type, params object[] args);
        #endregion

        #region Behaviour

        IBehaviourManager Behaviour { get; }
        XActivator Activator { get; }

        UniTask RunAsync(CancellationToken cancellationToken = default);
        void RunAsync(Action onFinish, Action<Exception> onError = null, CancellationToken cancellationToken = default);
        #endregion

    }
}
