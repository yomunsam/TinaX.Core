using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Core.Behaviours;
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
        void ConfigureServices(IServiceContainer services); //配置服务

        void ConfigureBehaviours(IBehaviourManager behaviour, IServiceContainer services); //配置行为


        UniTask<ModuleBehaviourResult> OnInitAsync(IServiceContainer services, CancellationToken cancellationToken);

        UniTask<ModuleBehaviourResult> OnStartAsync(IServiceContainer services, CancellationToken cancellationToken);

        UniTask OnRestartAsync(IServiceContainer services, CancellationToken cancellationToken);

        void OnQuit();

    }
}
