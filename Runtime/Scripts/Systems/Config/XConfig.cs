using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TinaX
{
    public static class XConfig
    {
        public static string ConfigResourcesPath { get;} = $"Assets/Resources/{ConfigRootFolderInResource}";

        public const string ConfigRootFolderInResource = "XConfig"; //以Resources.Load视角下的ResourcesPath;


        //private static List<>
        private const string mPathHeadText_LoadByResources = "ures:";
        private const string mPathHeadText_LoadByVFS = "xvfs:";

        /// <summary>
        /// key: path(附带头)， value: 缓存对象
        /// </summary>
        private static Dictionary<string, object> mDict_ConfigCache = new Dictionary<string, object>();
        private static Dictionary<string, object> mDict_JsonCache = new Dictionary<string, object>();

        public static void RefreshConfigs()
        {

        }
    
        /// <summary>
        /// Get Unity Config
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="loadType"></param>
        /// <param name="cachePrior">优先从缓存中读取</param>
        /// <returns></returns>
        public static T GetConfig<T>(string configPath , AssetLoadType loadType = AssetLoadType.Resources, bool cachePrior = true) where T : UnityEngine.ScriptableObject
        {
            bool cache_flag = false;
            string cache_key = ((loadType == AssetLoadType.Resources) ? mPathHeadText_LoadByResources : mPathHeadText_LoadByVFS) + configPath;
            if (cachePrior && UnityEngine.Application.isPlaying)
            {
                //检查缓存
                if (mDict_ConfigCache.TryGetValue(cache_key, out object conf))
                    return conf as T;
                else
                    cache_flag = true; //加载之后要加入缓存字典
            }

            T final_asset;
            //load asset 
            if(loadType == AssetLoadType.Resources)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    string _final_path = (configPath.StartsWith("/") ? ConfigRootFolderInResource + configPath : ConfigRootFolderInResource + "/" + configPath);
                    _final_path = RemoveExtNameIfExists(_final_path);
                    final_asset = Resources.Load<T>(_final_path);
                }
                else
                {
                    //在编辑器下应该使用AssetDatabase加载
                    if (configPath.IndexOf("Resources") != -1 && configPath.StartsWith("Assets/"))
                    {
                        //本来给定的就是unity asset path, 直接加载
                        final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(configPath);
                    }
                    else
                    {
                        //本来的路径是Resources的路径，要还原成Unity的路径
                        string uPath = ConfigResourcesPath + "/" + configPath;
                        if (!uPath.EndsWith(".asset"))
                            uPath = uPath + ".asset";

                        final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(uPath);
                    }
                }
#else
                string _final_path = (configPath.StartsWith("/") ? ConfigRootFolderInResource + configPath : ConfigRootFolderInResource + "/" + configPath);
                _final_path = RemoveExtNameIfExists(_final_path);
                final_asset = Resources.Load<T>(_final_path);
#endif
            }
            else if(loadType == AssetLoadType.VFS)
            {
#if UNITY_EDITOR
                if(Application.isPlaying && XCore.MainInstance != null && XCore.GetMainInstance().IsRunning)
                {
                    final_asset = XCore.GetMainInstance().GetService<TinaX.Services.IAssetService>().Load<T>(configPath);
                }
                else
                {
                    //editor only : load asset by assetdatabase.
                    final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(configPath);
                }

#else
                final_asset = XCore.GetMainInstance()?.GetService<TinaX.Services.IAssetService>()?.Load<T>(configPath);
#endif
            }
            else
            {
                throw new Exception($"[TinaX] XConfig System can't load asset by \"{loadType.ToString()}\"loadtype.");
            }

            if (cache_flag)
            {
                if (mDict_ConfigCache.ContainsKey(cache_key))
                    mDict_ConfigCache[cache_key] = final_asset;
                else
                    mDict_ConfigCache.Add(cache_key, final_asset);
            }

            return final_asset;

        }

        public static object GetConfig(string configPath, Type type, AssetLoadType loadType = AssetLoadType.Resources, bool cachePrior = true)
        {
            if (!type.IsSubclassOf(typeof(UnityEngine.ScriptableObject))) { Debug.LogError("[TinaX] XConfig : the config you want get is not valid \"ScriptableObject\"Type.  type: " + type.FullName + "  path:" + configPath); return null; }

            bool cache_flag = false;
            string cache_key = ((loadType == AssetLoadType.Resources) ? mPathHeadText_LoadByResources : mPathHeadText_LoadByVFS) + configPath;

            if (cachePrior && UnityEngine.Application.isPlaying)
            {
                //检查缓存
                if (mDict_ConfigCache.TryGetValue(cache_key, out object conf))
                    return conf;
                else
                    cache_flag = true; //加载之后要加入缓存字典
            }

            object final_asset;
            //load asset 
            if (loadType == AssetLoadType.Resources)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    string _final_path = (configPath.StartsWith("/") ? ConfigRootFolderInResource + configPath : ConfigRootFolderInResource + "/" + configPath);
                    _final_path = RemoveExtNameIfExists(_final_path);
                    final_asset = Resources.Load(_final_path, type);
                }
                else
                {
                    //在编辑器下应该使用AssetDatabase加载
                    if (configPath.IndexOf("Resources") != -1 && configPath.StartsWith("Assets/"))
                    {
                        //本来给定的就是unity asset path, 直接加载
                        final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath(configPath, type);
                    }
                    else
                    {
                        //本来的路径是Resources的路径，要还原成Unity的路径
                        string uPath = ConfigResourcesPath + "/" + configPath;
                        if (!uPath.EndsWith(".asset"))
                            uPath += ".asset";

                        final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath(uPath, type);
                    }
                }
                
#else
                string _final_path = (configPath.StartsWith("/") ? ConfigRootFolderInResource + configPath : ConfigRootFolderInResource + "/" + configPath);
                _final_path = RemoveExtNameIfExists(_final_path);
                final_asset = Resources.Load(_final_path, type);
#endif
            }
            else if (loadType == AssetLoadType.VFS)
            {
#if UNITY_EDITOR
                if (Application.isPlaying && XCore.MainInstance != null && XCore.GetMainInstance().IsRunning)
                {
                    final_asset = XCore.GetMainInstance().GetService<TinaX.Services.IAssetService>().Load(configPath,type);
                }
                else
                {
                    //editor only : load asset by assetdatabase.
                    final_asset = UnityEditor.AssetDatabase.LoadAssetAtPath(configPath,type);
                }

#else
                final_asset = XCore.GetMainInstance()?.GetService<TinaX.Services.IAssetService>()?.Load(configPath,type);
#endif
            }
            else
            {
                throw new Exception($"[TinaX] XConfig System can't load asset by \"{loadType.ToString()}\"loadtype.");
            }

            if (cache_flag)
            {
                if (mDict_ConfigCache.ContainsKey(cache_key))
                    mDict_ConfigCache[cache_key] = final_asset;
                else
                    mDict_ConfigCache.Add(cache_key, final_asset);
            }

            return final_asset;
        }

        public static T GetJson<T>(string jsonPath, AssetLoadType loadType = AssetLoadType.SystemIO, bool cachePrior = false)
        {
            bool cache_flag = false;
            string cache_key = jsonPath.ToLower();

            if (cachePrior && UnityEngine.Application.isPlaying)
            {
                //检查缓存
                if (mDict_JsonCache.TryGetValue(cache_key, out object conf))
                    return (T)conf;
                else
                    cache_flag = true; //加载之后要加入缓存字典
            }

            T final_asset;
            //load asset 
            if (loadType == AssetLoadType.Resources)
            {
#if UNITY_EDITOR
                //在编辑器下应该使用AssetDatabase加载
                if (jsonPath.IndexOf("Resources") != -1 && jsonPath.StartsWith("Assets/"))
                {
                    //本来给定的就是unity asset path, 直接加载
                    var json_ta = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                    final_asset = JsonUtility.FromJson<T>(json_ta.text);
                }
                else
                {
                    //本来的路径是Resources的路径，要还原成Unity的路径
                    string uPath = ConfigResourcesPath + "/" + jsonPath;
                    if (!uPath.EndsWith(".asset"))
                        uPath = uPath + ".asset";

                    var json_ta = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(uPath);
                    final_asset = JsonUtility.FromJson<T>(json_ta.text);
                }
#else
                var json_ta = Resources.Load<TextAsset>(RemoveExtNameIfExists(jsonPath));
                final_asset = JsonUtility.FromJson<T>(json_ta.text);
#endif
            }
            else if (loadType == AssetLoadType.VFS)
            {
#if UNITY_EDITOR
                if (Application.isPlaying && XCore.MainInstance != null && XCore.GetMainInstance().IsRunning)
                {
                    var json_textAsset = XCore.GetMainInstance().GetService<TinaX.Services.IAssetService>().Load<TextAsset>(jsonPath);
                    final_asset = JsonUtility.FromJson<T>(json_textAsset.text);
                }
                else
                {
                    //editor only : load asset by assetdatabase.
                    var json_textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                    final_asset = JsonUtility.FromJson<T>(json_textAsset.text);
                }

#else
                var json_textAsset = XCore.GetMainInstance()?.GetService<TinaX.Services.IAssetService>()?.Load<TextAsset>(jsonPath);
                final_asset = JsonUtility.FromJson<T>(json_textAsset.text);
#endif
            }
            else if(loadType == AssetLoadType.SystemIO)
            {
                var json_text = System.IO.File.ReadAllText(jsonPath, Encoding.UTF8);
                final_asset = JsonUtility.FromJson<T>(json_text);
            }
            else
            {
                throw new Exception($"[TinaX] XConfig System can't load asset by \"{loadType.ToString()}\"loadtype.");
            }

            if (cache_flag)
            {
                if (mDict_ConfigCache.ContainsKey(cache_key))
                    mDict_ConfigCache[cache_key] = final_asset;
                else
                    mDict_ConfigCache.Add(cache_key, final_asset);
            }

            return final_asset;
        }

        public static void SaveJson(object jsonContent,string json_path)
        {
            var json = JsonUtility.ToJson(jsonContent);
            TinaX.IO.XDirectory.CreateIfNotExists(System.IO.Path.GetDirectoryName(json_path));
            TinaX.IO.XFile.DeleteIfExists(json_path);
            System.IO.File.WriteAllText(json_path, json, Encoding.UTF8);
        }

#if UNITY_EDITOR

        public static T CreateConfigIfNotExists<T>(string configPath, AssetLoadType pathType = AssetLoadType.Resources) where T: ScriptableObject
        {
            UnityEditor.AssetDatabase.ReleaseCachedFileHandles();
            //检查路径
            var final_path = (pathType == AssetLoadType.Resources) ? (ConfigResourcesPath + "/" + configPath + ".asset") : configPath;
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(final_path);
            if(asset == null)
            {
                //create
                EnsureDirectory(final_path);
                var config = ScriptableObject.CreateInstance<T>();
                UnityEditor.AssetDatabase.CreateAsset(config, final_path);

                return config;
            }
            else
            {
                return asset;
            }
        }


        public static void SaveJson(object jsonContent, string jsonPath, AssetLoadType pathType)
        {
            //检查路径
            var final_unity_path = (pathType == AssetLoadType.Resources) ? (ConfigResourcesPath + "/" + jsonPath + ".json") : jsonPath;
            var system_path = System.IO.Path.GetFullPath(final_unity_path);
            if (System.IO.File.Exists(system_path))
            {
                System.IO.File.Delete(system_path) ;
            }
            var json_text = JsonUtility.ToJson(jsonContent);
            TinaX.IO.XDirectory.CreateIfNotExists(System.IO.Path.GetDirectoryName(system_path));
            System.IO.File.WriteAllText(system_path, json_text, Encoding.UTF8);
        }


        private static void EnsureDirectory(string unityPath)
        {
            TinaX.IO.XDirectory.CreateIfNotExists(
                System.IO.Path.Combine(
                    System.IO.Directory.GetCurrentDirectory(), 
                    System.IO.Path.GetDirectoryName(unityPath)
                    )
                );
        }

#endif

        /// <summary>
        /// 如果给定的路径有后缀名，则去掉后缀。
        /// </summary>
        /// <returns></returns>
        private static string RemoveExtNameIfExists(string source)
        {
            if (source.IsNullOrEmpty()) return source;
            int last_point = source.LastIndexOf('.');
            int last_slash = source.LastIndexOf('/');
            if (last_point > 0 && last_slash > 0 && last_slash > last_point)
                return source.Substring(0, last_point);
            else
                return source;
        }




    }
}
