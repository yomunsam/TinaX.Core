using System;
using System.Linq;
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
        public static XCore MainInstance => _MainInstance;
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

        public async Task RunAsync(System.Action finishCallback = null)
        {
            if (mInited) return;

            Debug.Log("[TinaX Framework] TinaX - v." + FrameworkVersionName + "    | Nekonya Studio\nhttps://github.com/yomunsam/tinax\nCorala.Space Project \n Powerd by yomunsam: https://yomunchan.moe | https://github.com/yomunsam");

            //在Scene创建一个全局的base gameobject
            //TODO: 如果在ECS模式，应该是不需要这么个东西的
            BaseGameObject = GameObjectHelper.FindOrCreateGo(FrameworkConst.Frameowrk_Base_GameObject_Name)
                .DontKillMe()
                .SetPosition(new Vector3(-6000, -6000, -6000));

            //catlib
            //mCatApp?.Init();
            
            //------------------触发Init阶段--------------------------------------------------------------
            //IXBootstrap获取启动引导
            var _b_type = typeof(IXBootstrap);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(_b_type)))
                .ToArray();
            foreach(var type in types)
                mList_XBootstrap.Add((IXBootstrap)Activator.CreateInstance(type));

            //Invoke IXBootstrap "Init"
            foreach(var item in mList_XBootstrap)
                item.OnInit();


            //Invoke Services "Init"
            foreach (var provider in mList_XServiceProviders)
            {
                Debug.Log("    [XService Init]:" + provider.ServiceName);
                var b = await provider.OnInit();
                if (!b)
                {
                    var e = provider.GetInitException();
                    if (mServicesInitExceptionAction != null)
                        mServicesInitExceptionAction.Invoke(provider.ServiceName, e);
                    else
                        throw e;

                }
            }


            //----------------------------------------------------------------------------------------------

            //Invoke Service "Register"
            foreach(var provider in mList_XServiceProviders)
                provider.OnServiceRegister();

            

            //------------------触发Start阶段----------------------------------------------------------------

            //Invoke Services "Start"
            foreach (var p in mList_XServiceProviders)
            {
                Debug.Log("    [XService Start]:" + p.ServiceName);
                var b = await p.OnStart();
                if (!b)
                {
                    var e = p.GetStartException();
                    if (mServicesStartExceptionAction != null)
                        mServicesStartExceptionAction.Invoke(p.ServiceName, e);
                    else
                        throw e;
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
            List<Task> task_close = new List<Task>();
            foreach (var provider in mList_XServiceProviders)
                task_close.Add(provider.OnClose());

            await Task.WhenAll(task_close);

            App.Terminate();

            IsRunning = false;
        } 
    
    }
}

