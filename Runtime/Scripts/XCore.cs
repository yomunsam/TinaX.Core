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
using UniRx;
using TinaX.Container;

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

        public IServiceContainer Services => m_ServiceContainer;

        

        private bool m_Inited = false;
        //private CatLib.Application mCatApp;

        private List<IXServiceProvider> m_XServiceProvidersList = new List<IXServiceProvider>();

        private List<IXBootstrap> m_XBootstrapList = new List<IXBootstrap>();

        /// <summary>
        /// 服务初始化失败的事件注册, string:service name 
        /// </summary>
        private Action<string, XException> m_ServicesInitExceptionAction;
        /// <summary>
        /// 服务启动失败的事件注册, string:service name 
        /// </summary>
        private Action<string, XException> m_ServicesStartExceptionAction;

        private ServiceContainer m_ServiceContainer;

        /// <summary>
        /// 是否尝试获取过IAppDomain
        /// </summary>
        internal bool m_TryGetIAppDomain = false;
        private IAppDomain m_IAppDomain;

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

            //mCatApp = CatLib.Application.New();
            m_ServiceContainer = new ServiceContainer(this);
            UnityEngine.Application.quitting += OnUnityQuit;
        }

        ~XCore()
        {
            UnityEngine.Application.quitting -= OnUnityQuit;
        }


        #region Dependency Injection | 依赖注入

        public IXCore RegisterServiceProvider(IXServiceProvider provider)
        {
            if (!m_XServiceProvidersList.Contains(provider))
                m_XServiceProvidersList.Add(provider);

            return this;
        }

        public void BindService<TService, TConcrete>()
        {
            m_ServiceContainer.Bind<TService, TConcrete>();
        }

        public void BindSingletonService<TService, TConcrete>()
        {
            m_ServiceContainer.Singleton<TService,TConcrete>();
        }
        
        

        public bool TryGetBuiltinService<TBuiltInInterface>(out TBuiltInInterface service) where TBuiltInInterface: IBuiltInService
            => m_ServiceContainer.TryGetBuildInService(out service);

        public TService GetService<TService>(params object[] userParams) 
            => m_ServiceContainer.Get<TService>(userParams);

        public object GetService(Type type, params object[] userParams)
            => m_ServiceContainer.Get(type, userParams);

        public bool TryGetService<TService>(out TService service, params object[] userParams)
            => m_ServiceContainer.TryGet<TService>(out service, userParams);

        public bool TryGetService(Type type, out object service, params object[] userParams)
            => m_ServiceContainer.TryGet(type, out service,userParams);

        public IXCore ConfigureServices(Action<IServiceContainer> options)
        {
            options?.Invoke(m_ServiceContainer);
            return this;
        }

        /// <summary>
        /// 对传入的类进行依赖注入
        /// </summary>
        /// <param name="target"></param>
        public void InjectObject(object target)
            => m_ServiceContainer.Inject(target);

        #endregion

        #region Exceptions | 异常处理
        public IXCore OnServicesInitException(Action<string,XException> callback)
        {
            m_ServicesInitExceptionAction += callback;
            return this;
        }

        public IXCore OnServicesStartException(Action<string, XException> callback)
        {
            m_ServicesStartExceptionAction += callback;
            return this;
        }

        #endregion

        #region Domains

        public object CreateInstance(Type type, params object[] args)
        {
            if (m_Inited)
            {
                if (!m_TryGetIAppDomain)
                {
                    Services.TryGetBuildInService(out m_IAppDomain);
                    m_TryGetIAppDomain = true;
                }

                if (m_IAppDomain != null)
                    return m_IAppDomain.CreateInstance(type, args);
            }

            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 仅供XCatApplication使用
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal string GetServiceName(Type type)
        {
            if (m_Inited)
            {
                if (!m_TryGetIAppDomain)
                {
                    Services.TryGetBuildInService(out m_IAppDomain);
                    m_TryGetIAppDomain = true;
                }

                if (m_IAppDomain != null)
                {
                    if (m_IAppDomain.TryGetServiceName(type, out var _name))
                        return _name;
                }
            }
            return Services.Type2ServiceName(type);
        }


        #endregion

        public async Task RunAsync()
        {
            if (m_Inited) return;

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

            m_XServiceProvidersList.Sort((x, y) => getServiceProviderOrder(ref x).CompareTo(getServiceProviderOrder(ref y)));

            //------------------触发Init阶段--------------------------------------------------------------
            //IXBootstrap获取启动引导
            var _b_type = typeof(IXBootstrap);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(_b_type)))
                .ToArray();
            foreach(var type in types)
                m_XBootstrapList.Add((IXBootstrap)Activator.CreateInstance(type));

            


            //Invoke Services "Init"
            Task<XException>[] arr_init_task = new Task<XException>[m_XServiceProvidersList.Count];
            for(int i = 0; i< m_XServiceProvidersList.Count; i++)
            {
                Debug.Log("    [XService Init]:" + m_XServiceProvidersList[i].ServiceName);
                arr_init_task[i] = m_XServiceProvidersList[i].OnInit(this);
            }
            await Task.WhenAll(arr_init_task);
            for(int i = 0; i < m_XServiceProvidersList.Count; i++)
            {
                var exception = arr_init_task[i].Result;
                if (exception != null)
                {
                    if (m_ServicesInitExceptionAction != null && m_ServicesInitExceptionAction.GetInvocationList().Length > 0)
                        m_ServicesInitExceptionAction?.Invoke(m_XServiceProvidersList[i].ServiceName, exception);
                    else
                    {
                        Debug.LogError($"[TinaX.Core] Exception when init xserver \"{m_XServiceProvidersList[i].ServiceName}\"");
                        throw exception;
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
            foreach (var provider in m_XServiceProvidersList)
                provider.OnServiceRegister(this);

            //--------------------------------------------------------------------------------------------------
            //Invoke IXBootstrap "Init"
            foreach (var item in m_XBootstrapList)
                item.OnInit(this);

            //------------------触发Start阶段----------------------------------------------------------------

            //Invoke Services "Start"
            foreach (var p in m_XServiceProvidersList)
            {
                Debug.Log("    [XService Start]:" + p.ServiceName);
                var exception = await p.OnStart(this);
                if (exception != null)
                {
                    
                    if (exception == null)
                        m_ServicesInitExceptionAction?.Invoke(p.ServiceName, null);
                    else
                    {
                        m_ServicesStartExceptionAction?.Invoke(p.ServiceName, exception);
#if UNITY_EDITOR
                        Debug.LogError($"Serivce provider \"{p.ServiceName}\" start exception: " + exception.Message);
#endif
                    }

                }
            }

            //------------------------------------------------------------------------------------------------

            Debug.Log("[TinaX] Framework startup finish.");
            IsRunning = true;
            m_Inited = true;

            //Invoke XBootstrap "Start"
            foreach (var xbs in m_XBootstrapList)
                xbs.OnStart(this);

            Debug.Log("[TinaX] App startup finish.");
        }
    
        public void RunAsync(Action<Exception> finishCallback)
        {
            this.RunAsync()
                .ToObservable()
                .ObserveOnMainThread()
                .Subscribe(unit =>
                {
                    finishCallback?.Invoke(null);
                },
                e =>
                {
                    Debug.LogException(e);
                    finishCallback?.Invoke(e);
                });
        }

        public async Task CloseAsync()
        {
            //Invoke XBootstrap "OnClose"
            foreach (var xbs in m_XBootstrapList)
                xbs.OnQuit();

            //Invoke Services "OnClose"
            foreach (var provider in m_XServiceProvidersList)
                provider.OnQuit();

            m_XBootstrapList.Clear();
            m_XServiceProvidersList.Clear();

            App.Terminate();
            IsRunning = false;
            await Task.Yield();
        } 
    


        private void OnUnityQuit()
        {
            if(m_XBootstrapList != null && m_XBootstrapList.Count > 0)
            {
                foreach(var item in m_XBootstrapList)
                    item.OnQuit();
            }
            m_XBootstrapList.Clear();

            if (m_XServiceProvidersList!= null && m_XServiceProvidersList.Count > 0)
            {
                foreach(var item in m_XServiceProvidersList)
                    item.OnQuit();
            }
            m_XServiceProvidersList.Clear();
        }
    }
}

