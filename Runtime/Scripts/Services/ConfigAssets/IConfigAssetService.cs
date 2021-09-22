using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Core.ConfigAssets.Pipelines;
using TinaX.Systems.Pipeline;
using UnityEngine;

namespace TinaX.Services.ConfigAssets
{
    public interface IConfigAssetService
    {
        /// <summary>
        /// 加载配置资产的Handler管线
        /// </summary>
        XPipeline<ILoadConfigAssetHandler> LoadConfigAssetPipeline { get; }
        XPipeline<ILoadConfigAssetAsyncHandler> LoadConfigAssetAsyncPipeline { get; }

        T GetConfig<T>(string loadPath) where T : ScriptableObject;
        UniTask<T> GetConfigAsync<T>(string loadPath, CancellationToken cancellationToken = default) where T : ScriptableObject;
    }
}
