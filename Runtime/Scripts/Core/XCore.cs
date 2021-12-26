/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Behaviours.Internal;
using TinaX.Container;
using TinaX.Core.Activator;
using TinaX.Core.Behaviours;
using TinaX.Core.Behaviours.Internal;
using TinaX.Core.Consts;
using TinaX.Core.ReflectionProvider;
using TinaX.Core.Serivces;
using TinaX.Core.Utils;
using TinaX.Exceptions;
using TinaX.Module;
using TinaX.Modules;
using TinaX.Modules.Internal;
using UniRx;
using UnityEngine;

namespace TinaX
{
    public class XCore : IXCore
    {
        private readonly ServiceContainer m_ServiceContainer;
        private readonly ModulesManager m_ModulesManager;
        private readonly XCoreBehaviourManager m_BehaviourManager;
        private readonly XActivator m_Activator;
        private readonly ReflectionProviderManager m_ReflectionProviderManager;
        private readonly bool m_IsHans;

        public XCore()
        {
            m_ServiceContainer = new ServiceContainer(this);
            m_ModulesManager = new ModulesManager();
            m_BehaviourManager = new XCoreBehaviourManager();
            m_Activator = new XActivator();
            m_ReflectionProviderManager = new ReflectionProviderManager();
            m_IsHans = LocalizationUtil.IsHans();
            CoreConfigureServices.ConfigureServices(m_ServiceContainer, this); //注册TinaX.Core包的服务
        }

        /// <summary>
        /// 启动XCore用的Task
        /// </summary>
        private UniTask? m_RunTask;
        private XBootstrapManager m_XBootstrapManager;

        #region Builds

        /// <summary>
        /// Reflect and register all <see cref="IXBootstrap"/> interfaces when startup
        /// </summary>
        public bool ReflectIXBootstrap { get; set; } = false;
         
        #endregion

        #region Instances

        private static IXCore _MainInstance;
        private static object _lock_obj = new object();
        public static IXCore MainInstance
        {
            get => _MainInstance;
            set
            {
                Debug.LogFormat("[{0}]The singleton \"MainInstance\" is modified: {1} -> {2}",
                    nameof(XCore),
                    _MainInstance == null ? "*Empty" : _MainInstance.GetHashCode().ToString(),
                    value == null ? "*Empty" : value.GetHashCode().ToString());
                _MainInstance = value;
            }
        }
        #endregion

        #region CreateInstances

        public static IXCore CreateDefault()
        {
            var core = new XCore();
            core.ReflectIXBootstrap = true;
            return core;
        }

        #endregion

        #region Reflection
        public ReflectionProviderManager ReflectionProviderManager => m_ReflectionProviderManager;
        #endregion


        #region Informations

        public string FrameworkVersionName => TinaXConst.Framework_VersionName;

        #endregion  

        #region Status

        public bool IsRunning { get; private set; }


        #endregion

        #region ServiceContainer
        public IServiceContainer Services => m_ServiceContainer;

        #endregion

        #region AppDomains
        public XActivator Activator => m_Activator;

        public object CreateInstance(Type type, params object[] args)
            => m_Activator.CreateInstance(type, args);
        #endregion

        #region Modules
        public IXCore AddModule(IModuleProvider moduleProvider)
        {
            m_ModulesManager.Add(moduleProvider);
            return this;
        }

        #endregion



        #region Behaviour

        public IBehaviourManager Behaviour => m_BehaviourManager;

        public UniTask RunAsync(CancellationToken cancellationToken = default)
        {
            if (m_RunTask == null)
            {
                lock (this)
                {
                    if(m_RunTask == null)
                    {
                        m_RunTask = DoRunAsync(cancellationToken).Preserve();
                    }
                }
            }

            return m_RunTask.Value;
        }

        public void RunAsync(Action onFinish, Action<Exception> onError = null, CancellationToken cancellationToken = default)
        {
            this.RunAsync(cancellationToken)
                .ToObservable()
                .SubscribeOnMainThread()
                .Subscribe(u => onFinish?.Invoke(), e => onError?.Invoke(e));
        }

        private async UniTask DoRunAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("[TinaX Framework] TinaX - v." + FrameworkVersionName + "    | Nekonya Studio\nhttps://github.com/yomunsam/tinax\n");

            //命令行参数

            //统一配置接口

            //IXBootstrap
            var enable_ixbootstrap = this.ReflectIXBootstrap;
            if (enable_ixbootstrap)
            {
                m_XBootstrapManager = new XBootstrapManager();
            }

            //模块 - 启动
            m_ModulesManager.Sort(); //排序一次

            var module_enumerator = m_ModulesManager.Providers.GetEnumerator();
            var init_task_list = new List<UniTask<ModuleBehaviourResult>>(m_ModulesManager.Providers.Count);
            while (module_enumerator.MoveNext())
            {
                module_enumerator.Current.ConfigureBehaviours(m_BehaviourManager, m_ServiceContainer); //注册行为

                init_task_list.Add(module_enumerator.Current.OnInitAsync(m_ServiceContainer, cancellationToken));
            }
            if(init_task_list.Count > 0)
            {
                var init_result = await UniTask.WhenAll(init_task_list);
                var err_results = init_result.Where(r => r.IsError).ToArray();
                if(err_results.Length > 0)
                {
                    Debug.LogErrorFormat("[{0}]Some exceptions were thrown when initializing the modules", nameof(XCore));
                    foreach (var err in err_results)
                    {
                        Debug.LogErrorFormat("[Module:{0}]{1}", err.ModuleName, err.Exception.Message);
                    }
                    m_RunTask = null;
                    throw err_results[0].Exception; //抛出第一个异常
                }
                
            }

            //模块 开始配置服务容器
            module_enumerator.Reset();
            while (module_enumerator.MoveNext())
            {
                module_enumerator.Current.ConfigureServices(m_ServiceContainer);
            }

            //Invoke IXBootstrap "Init"
            if(enable_ixbootstrap && m_XBootstrapManager != null)
            {
                var xbs_enumerator = m_XBootstrapManager.XBootstraps.GetEnumerator();
                while (xbs_enumerator.MoveNext())
                {
                    xbs_enumerator.Current.OnInit(this);
                }
            }

            //XCore Behaviour Awake
            await m_BehaviourManager.InvokeAwakeAsync();

            //模块 Start
            /*
             * 模块有严格的启动顺序要求，因此在TinaX 6.x的时候，会依次等待所有异步方法
             * 这里使用另一种做法，把所有模块根据Order做分组，然后对相同的Order作为一组一起加载，已组为单位做次序等待
             * 下面的这堆操作会产生一定的GC开销。
             * 测下来（并不是所有情况下）会比TinaX 6.x的做法快。
             */
            var order_index = m_ModulesManager.GetOrderList();
            var start_task_list = new List<UniTask<ModuleBehaviourResult>>();
            for (int i = 0; i < order_index.Count; i++)
            {
                start_task_list.Clear();
                var orderModule_enumerator = m_ModulesManager.OrderProviders[order_index[i]].GetEnumerator();
                while (orderModule_enumerator.MoveNext())
                {
                    start_task_list.Add(orderModule_enumerator.Current.OnStartAsync(m_ServiceContainer, cancellationToken));

                }
                if(start_task_list.Count > 0)
                {
                    try
                    {
                        var start_result = await UniTask.WhenAll(start_task_list);
                        var err_results = start_result.Where(r => r.IsError).ToArray();
                        if (err_results.Length > 0)
                        {
                            Debug.LogErrorFormat("[{0}]Some exceptions were thrown when starting the modules", nameof(XCore));
                            foreach (var err in err_results)
                            {
                                Debug.LogErrorFormat("[Module:{0}]{1}", err.ModuleName, err.Exception.Message);
                            }
                            m_RunTask = null;
                            return;
                        }
                    }
                    catch(XException ex)
                    {
                        Debug.LogErrorFormat("Start Modules Exception: [0]{1}", ex.ModuleName, ex.Message);
                        //Debug.LogException(ex);
                        throw ex; //抛出异常
                    }
                }
            }

            //------------------------------------------------------------------------------------------------

            if (m_IsHans)
                Debug.Log("[TinaX] 框架启动完成");
            else
                Debug.Log("[TinaX] Framework startup finish.");
            IsRunning = true;
            if (_MainInstance == null)
            {
                lock (_lock_obj)
                {
                    if(_MainInstance == null)
                        _MainInstance = this;
                }
            }

            //Invoke XBootstrap "Start"
            if (enable_ixbootstrap && m_XBootstrapManager != null)
            {
                var xbs_enumerator = m_XBootstrapManager.XBootstraps.GetEnumerator();
                while (xbs_enumerator.MoveNext())
                {
                    xbs_enumerator.Current.OnStart(this);
                }
            }

            //XCore Behaviour Start
            await m_BehaviourManager.InvokeStartAsync();

            m_RunTask = UniTask.CompletedTask;
            Debug.Log("[TinaX] App startup finish.");
        }


        #endregion

    }
}
