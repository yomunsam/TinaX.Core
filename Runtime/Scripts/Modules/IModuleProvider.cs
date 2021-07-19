using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Modules;

namespace TinaX.Module
{
    /// <summary>
    /// TinaX Module Provider 
    /// 模块提供者
    /// </summary>
    public interface IModuleProvider
    {
        string ModuleName { get; }

        /// <summary>
        /// after "OnInit" and before "OnStart" 
        /// </summary>
        void ConfigureServices(IServiceContainer services);

        UniTask<ModuleBehaviourResult> OnInit(IServiceContainer services, CancellationToken cancellationToken);

        UniTask<ModuleBehaviourResult> OnStart(IServiceContainer services, CancellationToken cancellationToken);

        void OnQuit();
        UniTask OnRestart(IServiceContainer services, CancellationToken cancellationToken);

    }
}
