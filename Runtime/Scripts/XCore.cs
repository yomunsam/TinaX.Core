using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CatLib;
using CatLib.Container;
using TinaX.Services;
using TinaX.Const;

namespace TinaX
{
    public class XCore : IXCore
    {
        #region instance and create 

        private static XCore _MainInstance;
        private static object _lock_obj = new object();
        public static IXCore MainInstance => _MainInstance;
        public static IXCore New()
        {
            if (_MainInstance == null)
            {
                _MainInstance = new XCore();
                return MainInstance;
            }
            else
                return new XCore();
        }

        public static IXCore GetMainInstance() => XCore.MainInstance;

        #endregion

        public XCore()
        {
            if (_MainInstance == null)
            {
                lock (_lock_obj)
                {
                    if (_MainInstance == null)
                        _MainInstance = this;
                }
            }

            mCatApp = CatLib.Application.New();
            UnityEngine.Application.quitting += OnUnityQuit;
        }

        ~XCore()
        {
            UnityEngine.Application.quitting -= OnUnityQuit;
        }

        public GameObject BaseGameObject { get; private set; }

        public string FrameworkVersionName => FrameworkConst.Framework_VersionName;

        /// <summary>
        /// 框架的沙箱存储路径
        /// </summary>
        public string LocalStoragePath_TinaX => XCore.LocalStorage_TinaX;

        public static string LocalStorage_TinaX => UnityEngine.Application.persistentDataPath + "/" + FrameworkConst.Framework_LocalStorage_TinaX;


        /// <summary>
        /// App的沙箱存储路径（提供给业务逻辑开发者）
        /// </summary>
        public string LocalStoragePath_App => XCore.LocalStorage_App;


        public static string LocalStorage_App => UnityEngine.Application.persistentDataPath + "/" + FrameworkConst.Framework_LocalStorage_App;

        public bool IsRunning { get; private set; } = false;

        public string ProfileName { get; private set; } = FrameworkConst.DefaultProfileName;
        public bool DevelopMode { get; private set; } = false;

        private bool mInited = false;
        private CatLib.Application mCatApp;

        private List<IXServiceProvider> mList_XServiceProviders = new List<IXServiceProvider>();

        private List<IXBootstrap> mList_XBootstrap = new List<IXBootstrap>();

        /// <summary>
        /// 服务初始化失败的事件注册, string:service name 
        /// </summary>
        private Action<string, XException> mServicesInitExceptionAction;
        /// <summary>
        /// 服务启动失败的事件注册, string:service name 
        /// </summary>
        private Action<string, XException> mServicesStartExceptionAction;

        #region Dependency Injection | 依赖注入

        public IXCore RegisterServiceProvider(IXServiceProvider provider)
        {
            if (!mList_XServiceProviders.Contains(provider))
                mList_XServiceProviders.Add(provider);

            return this;
        }

        public void BindService<TService, TConcrete>()
        {
            App.Bind<TService, TConcrete>();
        }

        public IBindData BindSingletonService<TService, TConcrete>()
        {
            return App.Singleton<TService, TConcrete>();
        }
        


        public IBindData BindSingletonService<TService,TBuiltInInterface, TConcrete>() where TBuiltInInterface : IBuiltInService
        {
            return App.Singleton<TService, TConcrete>().Alias<TBuiltInInterface>();
        }

        public bool TryGetBuiltinService<TBuiltInInterface>(out TBuiltInInterface service) where TBuiltInInterface: IBuiltInService
        {
            if (App.IsAlias<TBuiltInInterface>())
            {
                service =  App.Make<TBuiltInInterface>();
                return true;
            }

            service = default;
            return false;
        }

        public bool IsBuiltInServicesImplementationed<TBuiltInInterface>() where TBuiltInInterface : IBuiltInService => App.IsAlias<TBuiltInInterface>();

        public TService GetService<TService>(params object[] userParams) => App.Make<TService>(userParams);

        public object GetService(Type type, params object[] userParams)
        {
            string service_name = mCatApp.Type2Service(type);
            if (mCatApp.IsStatic(service_name))
                return mCatApp.Make(service_name, userParams);
            if (mCatApp.IsAlias(service_name))
                return mCatApp.Make(service_name, userParams);
            return null;
        }

        public bool TryGetService<TService>(out TService service, params object[] userParams)
        {
            if (mCatApp.IsStatic<TService>())
            {
                service = mCatApp.Make<TService>(userParams);
                return true;
            }
            if (mCatApp.IsAlias<TService>())
            {
                service = mCatApp.Make<TService>(userParams);
                return true;
            }
            service = default;
            return false;
        }

        /// <summary>
        /// 对传入的类进行依赖注入
        /// </summary>
        /// <param name="obj"></param>
        public void InjectObject(object obj)
        {
            Type obj_type = obj.GetType();
            foreach(var field in obj_type.GetRuntimeFields())
            {
                var attr = field.GetCustomAttribute<InjectAttribute>(true);
                if (attr == null)
                    continue;
                var service_name = mCatApp.Type2Service(field.FieldType);
                if(mCatApp.IsStatic(service_name))
                {
                    field.SetValue(obj, mCatApp.Make(service_name));
                    continue;
                }

                if (mCatApp.IsAlias(service_name))
                {
                    field.SetValue(obj, mCatApp.Make(service_name));
                    continue;
                }

                if (attr.Nullable)
                    continue;
                else
                    throw new ServiceNotFoundException(field.FieldType); //抛异常
            }

            foreach(var property in obj_type.GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<InjectAttribute>(true);
                if (attr == null)
                    continue;
                var service_name = mCatApp.Type2Service(property.PropertyType);
                if (mCatApp.IsStatic(service_name))
                {
                    property.SetValue(obj, mCatApp.Make(service_name));
                    continue;
                }

                if (mCatApp.IsAlias(service_name))
                {
                    property.SetValue(obj, mCatApp.Make(service_name));
                    continue;
                }

                if (attr.Nullable)
                    continue;
                else
                    throw new ServiceNotFoundException(property.PropertyType); //抛异常
            }
        }

        #endregion

        #region Exceptions | 异常处理
        public IXCore OnServicesInitException(Action<string,XException> callback)
        {
            mServicesInitExceptionAction += callback;
            return this;
        }

        public IXCore OnServicesStartException(Action<string, XException> callback)
        {
            mServicesStartExceptionAction += callback;
            return this;
        }

        #endregion

        #region Domains

        public object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }



        #endregion

        public async Task RunAsync(System.Action finishCallback = null)
        {
            if (mInited) return;

            Debug.Log("[TinaX Framework] TinaX - v." + FrameworkVersionName + "    | Nekonya Studio\nhttps://github.com/yomunsam/tinax\nCorala.Space Project \n Powerd by yomunsam: https://yomunchan.moe | https://github.com/yomunsam");

            //在Scene创建一个全局的base gameobject
            //TODO: 如果在ECS模式，应该是不需要这么个东西的
            BaseGameObject = GameObjectHelper.FindOrCreateGameObject(FrameworkConst.Frameowrk_Base_GameObject_Name)
                .DontKillMe()
                .SetPosition(new Vector3(-6000, -6000, -6000));

            //Profile
            var profile = XConfig.GetConfig<TinaX.Internal.XProfileConfig>(FrameworkConst.XProfile_Config_Path);
            if(profile != null)
            {
                this.ProfileName = profile.ActiveProfileName;
                this.DevelopMode = profile.DevelopMode;
            }

            
            //对Service进行排序
            int getServiceProviderOrder(ref IXServiceProvider provider)
            {
                Type p_type = provider.GetType();
                var attr = p_type.GetCustomAttribute<XServiceProviderOrderAttribute>();
                if (attr != null)
                    return attr.Order;
                else
                    return 100;
            }

            mList_XServiceProviders.Sort((x, y) => getServiceProviderOrder(ref x).CompareTo(getServiceProviderOrder(ref y)));

            //------------------触发Init阶段--------------------------------------------------------------
            //IXBootstrap获取启动引导
            var _b_type = typeof(IXBootstrap);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(_b_type)))
                .ToArray();
            foreach(var type in types)
                mList_XBootstrap.Add((IXBootstrap)Activator.CreateInstance(type));

            


            //Invoke Services "Init"
            Task<bool>[] arr_init_task = new Task<bool>[mList_XServiceProviders.Count];
            for(int i = 0; i< mList_XServiceProviders.Count; i++)
            {
                Debug.Log("    [XService Init]:" + mList_XServiceProviders[i].ServiceName);
                arr_init_task[i] = mList_XServiceProviders[i].OnInit();
            }
            await Task.WhenAll(arr_init_task);
            for(int i = 0; i < mList_XServiceProviders.Count; i++)
            {
                if(!arr_init_task[i].Result)
                {
                    var e = mList_XServiceProviders[i].GetInitException();
                    if (mServicesInitExceptionAction != null && mServicesInitExceptionAction.GetInvocationList().Length > 0)
                        mServicesInitExceptionAction?.Invoke(mList_XServiceProviders[i].ServiceName, e);
                    else
                    {
                        Debug.LogError($"[TinaX.Core] Exception when init xserver \"{mList_XServiceProviders[i].ServiceName}\"");
                        throw e;
                    }
                }
            }

            //试试看下面这个和上面这个实际跑起来哪个快
            //foreach (var provider in mList_XServiceProviders)
            //{
            //    Debug.Log("    [XService Init]:" + provider.ServiceName);
            //    var b = await provider.OnInit();
            //    if (!b)
            //    {
            //        var e = provider.GetInitException();
            //        if (mServicesInitExceptionAction != null)
            //            mServicesInitExceptionAction.Invoke(provider.ServiceName, e);
            //        else
            //            throw e;

            //    }
            //}

            

            //----------------------------------------------------------------------------------------------

            //Invoke Service "Register"
            foreach (var provider in mList_XServiceProviders)
                provider.OnServiceRegister();

            //--------------------------------------------------------------------------------------------------
            //Invoke IXBootstrap "Init"
            foreach (var item in mList_XBootstrap)
                item.OnInit();

            //------------------触发Start阶段----------------------------------------------------------------

            //Invoke Services "Start"
            foreach (var p in mList_XServiceProviders)
            {
                Debug.Log("    [XService Start]:" + p.ServiceName);
                var b = await p.OnStart();
                if (!b)
                {
                    var e = p.GetStartException();
                    if (e == null)
                        mServicesInitExceptionAction?.Invoke(p.ServiceName, null);
                    else
                    {
                        mServicesStartExceptionAction?.Invoke(p.ServiceName, e);
#if UNITY_EDITOR
                        Debug.LogException(e);
#endif
                    }

                }
            }

            //------------------------------------------------------------------------------------------------

            Debug.Log("[TinaX] Framework startup finish.");
            IsRunning = true;


            //Invoke XBootstrap "Start"
            foreach (var xbs in mList_XBootstrap)
                xbs.OnStart();
            


            Debug.Log("[TinaX] App startup finish.");
            finishCallback?.Invoke();
        }
    
        public async Task CloseAsync()
        {
            //Invoke XBootstrap "OnClose"
            foreach (var xbs in mList_XBootstrap)
                xbs.OnQuit();

            //Invoke Services "OnClose"
            foreach (var provider in mList_XServiceProviders)
                provider.OnQuit();

            mList_XBootstrap.Clear();
            mList_XServiceProviders.Clear();

            App.Terminate();
            IsRunning = false;
            await Task.Yield();
        } 
    


        private void OnUnityQuit()
        {
            if(mList_XBootstrap != null && mList_XBootstrap.Count > 0)
            {
                foreach(var item in mList_XBootstrap)
                    item.OnQuit();
            }
            mList_XBootstrap.Clear();

            if (mList_XServiceProviders!= null && mList_XServiceProviders.Count > 0)
            {
                foreach(var item in mList_XServiceProviders)
                    item.OnQuit();
            }
            mList_XServiceProviders.Clear();
        }
    }
}

