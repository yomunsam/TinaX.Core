using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Catlib;
using TinaX.Container;
using TinaX.Core.Consts;
using TinaX.Module;
using TinaX.Modules;
using TinaX.Modules.Internal;
using UnityEngine;

namespace TinaX
{
    public class XCore : IXCore
    {
        private readonly ServiceContainer m_ServiceContainer;
        private readonly ModulesManager m_ModulesManager;

        public XCore()
        {
            m_ServiceContainer = new ServiceContainer(this);
            m_ModulesManager = new ModulesManager();
        }

        /// <summary>
        /// 启动XCore用的Task
        /// </summary>
        private UniTask? m_RunTask;


        #region Builds


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

            return core;
        }

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

        #region Modules
        public IXCore AddModule(IModuleProvider moduleProvider)
        {
            m_ModulesManager.Add(moduleProvider);
            return this;
        }

        #endregion


        #region Behaviour

        public UniTask RunAsync(CancellationToken cancellationToken = default)
        {
            if (m_RunTask == null)
            {
                lock (this)
                {
                    if(m_RunTask == null)
                    {
                        m_RunTask = DoRunAsync().Preserve();
                    }
                }
            }

            return m_RunTask.Value;
        }

        private async UniTask DoRunAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("[TinaX Framework] TinaX - v." + FrameworkVersionName + "    | Nekonya Studio\nhttps://github.com/yomunsam/tinax\n");

            //命令行参数

            //统一配置接口

            //模块 - 启动
            m_ModulesManager.Sort(); //排序一次

            var module_enumerator = m_ModulesManager.Providers.GetEnumerator();
            var init_task_list = new List<UniTask<ModuleBehaviourResult>>();
            while (module_enumerator.MoveNext())
            {
                init_task_list.Add(module_enumerator.Current.OnInit(m_ServiceContainer, cancellationToken));
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
                    return;
                }
                
            }

            //模块 开始配置服务容器
            module_enumerator.Reset();
            while (module_enumerator.MoveNext())
            {
                module_enumerator.Current.ConfigureServices(m_ServiceContainer);
            }

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
                    start_task_list.Add(orderModule_enumerator.Current.OnStart(m_ServiceContainer, cancellationToken));

                }
                if(start_task_list.Count > 0)
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
            }

            //------------------------------------------------------------------------------------------------

            Debug.Log("[TinaX] Framework startup finish.");
            IsRunning = true;
            m_RunTask = UniTask.CompletedTask;
        }


        #endregion

    }
}
