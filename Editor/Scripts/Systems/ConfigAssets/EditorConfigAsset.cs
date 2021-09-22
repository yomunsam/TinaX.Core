using System;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.Core
{
    /// <summary>
    /// 配置资产 编辑器静态类
    /// </summary>
    public static class EditorConfigAsset
    {
        public const string ConfigAssetsDirectoryInResources = @"Assets/Resources/TinaX/Configurations";

        public const string ConfigAssetsDirectoryNoResources = @"Assets/TinaX/Configurations";

        /// <summary>
        /// 存放配置资产的根目录
        /// </summary>
        public static string ConfigAssetsDirectory
        {
            get
            {
#if TINAX_CONFIG_NO_RESOURCES
                return ConfigAssetsDirectoryNoResources;
#else
                return ConfigAssetsDirectoryInResources;
#endif
            }
        }


        public static T GetConfig<T>(string loadPath) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(loadPath))
                throw new ArgumentNullException(nameof(loadPath));

            var final_path = GetResourcesLoadPathFromDefaultConfigFolder(loadPath);
#if TINAX_DEV
            Debug.LogFormat("[EditorConfigAsset] 加载配置资产:[{0}] {1}", typeof(T).Name, final_path);
#endif
            return AssetDatabase.LoadAssetAtPath<T>(loadPath);
        }


        /// <summary>
        /// 获取在TinaX默认存放配置资产的文件夹中的某个资产的Resources类的加载路径.
        /// 例如传入"myConf", 返回"Assets/Resources/TinaX/Configurations/myConf.asset" 或 "Assets/TinaX/Configurations/myConf.asset"
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        public static string GetResourcesLoadPathFromDefaultConfigFolder(string loadPath)
            => loadPath.StartsWith("/")
                ? $"{ConfigAssetsDirectory}{(loadPath.EndsWith(".asset") ? loadPath : string.Format("{0}.asset", loadPath))}"
                : $"{ConfigAssetsDirectory}/{(loadPath.EndsWith(".asset") ? loadPath : string.Format("{0}.asset", loadPath))}";

    }
}
