using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Core.ConfigAssets.Pipelines;
using TinaX.Exceptions;
using TinaX.Systems.Pipeline;
using UnityEngine;

namespace TinaX.Services.ConfigAssets
{
    public class ConfigAssetService : IConfigAssetService
    {

        public const string ConfigAssetsDirectoryForAssetServiceNoResources = @"Assets/TinaX/Configurations";
        public const string ConfigAssetsDirectoryForAssetServiceInResources = @"Assets/Resources/TinaX/Configurations";
        public const string ConfigAssetsDirectoryForResources = @"TinaX/Configurations";

        private readonly XPipeline<ILoadConfigAssetHandler> m_LoadConfigAssetPipeline = new XPipeline<ILoadConfigAssetHandler>();
        private readonly XPipeline<ILoadConfigAssetAsyncHandler> m_LoadConfigAssetAsyncPipeline = new XPipeline<ILoadConfigAssetAsyncHandler>();
        private readonly IXCore m_XCore;

        public ConfigAssetService(IXCore core)
        {
            this.m_XCore = core;
            LoadConfigAssetPipelineConfigure(ref m_LoadConfigAssetPipeline);
            LoadConfigAssetAsyncPipelineConfigure(ref m_LoadConfigAssetAsyncPipeline);
        }

        public XPipeline<ILoadConfigAssetHandler> LoadConfigAssetPipeline => m_LoadConfigAssetPipeline;
        public XPipeline<ILoadConfigAssetAsyncHandler> LoadConfigAssetAsyncPipeline => m_LoadConfigAssetAsyncPipeline;

        public T GetConfig<T>(string loadPath) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            LoadConfigAssetPayload payload = new LoadConfigAssetPayload()
            {
                LoadPath = loadPath,
                AssetType = typeof(T),
            };

            if(m_XCore.Services.TryGet<IAssetService>(out var assetService))
            {
                payload.LoadFormAssetService = true;
                payload.AssetService = assetService;
            }

            m_LoadConfigAssetPipeline.Start((handler, next) =>
            {
                return handler.LoadConfigAsset(ref payload, next);
            });
            return payload.LoadedAsset as T;
        }

        public async UniTask<T> GetConfigAsync<T>(string loadPath, CancellationToken cancellationToken = default) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            LoadConfigAssetPayload payload = new LoadConfigAssetPayload()
            {
                LoadPath = loadPath,
                AssetType = typeof(T),
            };

            if (m_XCore.Services.TryGet<IAssetService>(out var assetService))
            {
                payload.LoadFormAssetService = true;
                payload.AssetService = assetService;
            }

            await m_LoadConfigAssetAsyncPipeline.StartAsync(async (handler, next) =>
            {
                return await handler.LoadConfigAssetAsync(payload, next, cancellationToken);
            });

            return payload.LoadedAsset as T;
        }


        private void LoadConfigAssetPipelineConfigure(ref XPipeline<ILoadConfigAssetHandler> pipeline)
        {
            //准备 从资产服务接口加载资产
            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromAssetService, (ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next) =>
            {
                if (!payload.LoadFormAssetService)
                    return true;
                //处理加载路径
#if TINAX_CONFIG_NO_RESOURCES
                /*
                 * TinaX的配置不放在Resources目录里，这时候我们直接从默认约定的目录里返回加载路径即可,
                 * 如加载路径为"myConf",则返回"Assets/TinaX/Configurations/myConf.asset"
                 */
                payload.AssetServiceLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForAssetServiceNoResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForAssetServiceNoResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#else
                /*
                 * 这时候默认配置文件是存放在Resources中的，我们默认返回带Resources的路径
                 * 如加载路径为"myConf",则返回"Assets/Resources/TinaX/Configurations/myConf.asset"
                 */
                payload.AssetServiceLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForAssetServiceInResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForAssetServiceInResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#endif

                return true;
            });

            //从内置资产服务中加载
            pipeline.Use(LoadConfigAssetHandlerNameConsts.LoadFromAssetService, (ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next) =>
            {
                if (!payload.LoadFormAssetService)
                    return true;
                if(payload.AssetService == null)
                {
                    Debug.LogErrorFormat("[0]Load config asset from \"IAssetService\", but asset service not ready.", nameof(ConfigAssetService));
                    return true;
                }


#if TINAX_DEV
                Debug.LogFormat("[0]Load config asset from built-in asset service: [1]{2}", nameof(ConfigAssetService), payload.AssetType.Name, payload.AssetServiceLoadPath);
#endif
                try
                {
                    payload.LoadedAsset = payload.AssetService.Load(payload.AssetServiceLoadPath, payload.AssetType);
                }
                catch (NotFoundException)
                {
                    payload.LoadedAsset = null;
                    return true; //没加载到，继续往下走流程。
                }
                catch
                {
                    throw;
                }

                return payload.LoadedAsset == null; //如果加载到了什么，就中断加载流程。
            });

            //准备 从Unity的Resources类中加载资产
            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromResources, (ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next) =>
            {
#if TINAX_CONFIG_NO_RESOURCES
                payload.LoadFromResources = false;
#else
                payload.LoadFromResources = true;
                payload.ResourcesLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#endif
                return true;
            });

            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromResources, (ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next) =>
            {
                if (!payload.LoadFromResources)
                    return true;
#if TINAX_CONFIG_NO_RESOURCES
                Debug.LogErrorFormat("[0]Will load objects via the Resources static class, which is not expected.", nameof(ConfigAssetService));
#else
                payload.LoadedAsset = Resources.Load(payload.ResourcesLoadPath, payload.AssetType);
#endif
                return payload.LoadedAsset == null; //如果加载到了什么，就中断加载流程。
            });
        }

        private void LoadConfigAssetAsyncPipelineConfigure(ref XPipeline<ILoadConfigAssetAsyncHandler> pipeline)
        {
            //准备 从资产服务接口加载资产
            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromAssetService, (LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken) =>
            {
                if (!payload.LoadFormAssetService)
                    return UniTask.FromResult(true);
                //处理加载路径
#if TINAX_CONFIG_NO_RESOURCES
                /*
                 * TinaX的配置不放在Resources目录里，这时候我们直接从默认约定的目录里返回加载路径即可,
                 * 如加载路径为"myConf",则返回"Assets/TinaX/Configurations/myConf.asset"
                 */
                payload.AssetServiceLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForAssetServiceNoResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForAssetServiceNoResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#else
                /*
                 * 这时候默认配置文件是存放在Resources中的，我们默认返回带Resources的路径
                 * 如加载路径为"myConf",则返回"Assets/Resources/TinaX/Configurations/myConf.asset"
                 */
                payload.AssetServiceLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForAssetServiceInResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForAssetServiceInResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#endif

                return UniTask.FromResult(true);
            });

            //从内置资产服务中加载
            pipeline.Use(LoadConfigAssetHandlerNameConsts.LoadFromAssetService, async (LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken) =>
            {
                if (!payload.LoadFormAssetService)
                    return true;
                if (payload.AssetService == null)
                {
                    Debug.LogErrorFormat("[0]Load config asset from \"IAssetService\", but asset service not ready.", nameof(ConfigAssetService));
                    return true;
                }


#if TINAX_DEV
                Debug.LogFormat("[{0}]Load config asset async from built-in asset service: [{1}]{2}", nameof(ConfigAssetService), payload.AssetType.Name, payload.AssetServiceLoadPath);
#endif
                try
                {
                    payload.LoadedAsset = await payload.AssetService.LoadAsync(payload.AssetServiceLoadPath, payload.AssetType, cancellationToken);
                }
                catch (NotFoundException)
                {
                    payload.LoadedAsset = null;
                    return true; //没加载到，继续往下走流程。
                }
                catch
                {
                    throw;
                }

                return payload.LoadedAsset == null; //如果加载到了什么，就中断加载流程。
            });

            //准备 从Unity的Resources类中加载资产
            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromResources, (LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken) =>
            {
#if TINAX_CONFIG_NO_RESOURCES
                payload.LoadFromResources = false;
#else
                payload.LoadFromResources = true;
                payload.ResourcesLoadPath = payload.LoadPath.StartsWith("/")
                    ? $"{ConfigAssetsDirectoryForResources}{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}"
                    : $"{ConfigAssetsDirectoryForResources}/{(payload.LoadPath.EndsWith(".asset") ? payload.LoadPath : string.Format("{0}.asset", payload.LoadPath))}";
#endif
                return UniTask.FromResult(true);
            });

            pipeline.Use(LoadConfigAssetHandlerNameConsts.ReadyLoadFromResources, async (LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken) =>
            {
                if (!payload.LoadFromResources)
                    return true;
#if TINAX_CONFIG_NO_RESOURCES
                Debug.LogErrorFormat("[0]Will load objects via the Resources static class, which is not expected.", nameof(ConfigAssetService));
                await UniTask.CompletedTask;
#else
                payload.LoadedAsset = await Resources.LoadAsync(payload.ResourcesLoadPath, payload.AssetType).WithCancellation(cancellationToken);
#endif
                return payload.LoadedAsset == null; //如果加载到了什么，就中断加载流程。
            });

        }



    }
}
