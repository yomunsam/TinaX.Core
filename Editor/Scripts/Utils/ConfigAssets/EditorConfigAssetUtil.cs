using System;
using TinaX.Core.Consts;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.Core.Utils
{
    public static class EditorConfigAssetUtil
    {
        public const string ConfigAssetsEditorPath = @"Assets/Resources/";

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

            return AssetDatabase.LoadAssetAtPath<T>(loadPath);
        }


        /// <summary>
        /// 获取在TinaX默认存放配置资产的文件夹中的某个资产的Resources类的加载路径.
        /// 例如传入"myConf", 返回"Assets/Resources/TinaX/Configurations/myConf"
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        public static string GetResourcesLoadPathFromDefaultConfigFolder(string loadPath)
            => loadPath.StartsWith("/")
                ? $"{ConfigAssetsEditorPath}{TinaXConst.DefaultConfigAssetsFolderPath}{loadPath}"
                : $"{ConfigAssetsEditorPath}{TinaXConst.DefaultConfigAssetsFolderPath}/{loadPath}";
    }
}
