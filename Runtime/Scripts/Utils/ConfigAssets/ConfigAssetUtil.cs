using System;
using Cysharp.Threading.Tasks;
using TinaX.Core.Consts;
using UnityEngine;

namespace TinaX.Core.Utils
{
    public static class ConfigAssetUtil
    {
        //public const string ConfigAssetsEditorPath = @"Assets/Resources/";

        /// <summary>
        /// 从默认存放配置资产的文件夹中加载配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadPath">相对于默认文件夹的加载路径</param>
        /// <returns></returns>
        public static T GetConfigFromDefaultFolder<T>(string loadPath) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            var final_path = GetResourcesLoadPathFromDefaultConfigFolder(loadPath);

            return Resources.Load<T>(final_path);
        }

        public static async UniTask<T> GetConfigFromDefaultFolderAsync<T>(string loadPath) where T :ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            var final_path = GetResourcesLoadPathFromDefaultConfigFolder(loadPath);
            var asset = await Resources.LoadAsync<T>(final_path);
            await UniTask.SwitchToMainThread();
            return asset as T;
        }

        public static async UniTask<T> GetConfigAsync<T>(string loadPath) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            var asset = await Resources.LoadAsync<T>(loadPath);
            await UniTask.SwitchToMainThread();
            return asset as T;
        }



        /// <summary>
        /// 获取在TinaX默认存放配置资产的文件夹中的某个资产的Resources类的加载路径.
        /// 例如传入"myConf", 返回"TinaX/Configurations/myConf"
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        public static string GetResourcesLoadPathFromDefaultConfigFolder(string loadPath)
            => loadPath.StartsWith("/")
                ? $"{TinaXConst.DefaultConfigAssetsFolderPath}{loadPath}"
                : $"{TinaXConst.DefaultConfigAssetsFolderPath}/{loadPath}";

    }
}
