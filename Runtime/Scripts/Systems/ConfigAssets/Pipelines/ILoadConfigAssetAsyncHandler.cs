using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.Core.ConfigAssets.Pipelines
{
    /// <summary>
    /// 加载配置资产 异步 处理器
    /// </summary>
    public interface ILoadConfigAssetAsyncHandler
    {
        string HandlerName { get; }

        /// <summary>
        /// 异步加载配置资产
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="next"></param>
        /// <returns>若返回false则中断pipeline流程</returns>
        UniTask<bool> LoadConfigAssetAsync(LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken = default);
    }
}
