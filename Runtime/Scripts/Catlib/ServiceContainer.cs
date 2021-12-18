using System;
using System.Collections.Generic;
using CatLib.Container;
using TinaX.Container;
using TinaX.Core.Container;
using TinaX.Services.Builtin.Base;

namespace TinaX.Catlib
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly XCore m_Core;

        private readonly List<IServiceInjector> m_ServiceInjects = new List<IServiceInjector>();

        public ServiceContainer(XCore core)
        {
            m_Core = core;
            CatApp = new XCatApplication(core, this);

            CatApp.Instance<IXCore>(core);
        }

        public XCatApplication CatApp { get;}




        #region 构建获取服务
        public TService Get<TService>(params object[] userParams)
            => CatApp.Make<TService>(userParams);

        public object Get(Type type, params object[] userParams)
            => CatApp.Make(type, userParams);

        public object Get(string serviceName, params object[] userParams)
            => CatApp.Make(serviceName, userParams);

        public bool TryGet<TService>(out TService service, params object[] userParams)
        {
            if(CatApp.CanMake<TService>())
            {
                service = CatApp.Make<TService>(userParams);
                return true;
            }
            else
            {
                service = default;
                return false;
            }
        }

        public bool TryGet(Type type, out object service, params object[] userParams)
        {
            string service_name = CatApp.Type2Service(type);
            if (CatApp.CanMake(service_name))
            {
                service = CatApp.Make(service_name,userParams);
                return true;
            }
            else
            {
                service = default;
                return false;
            }
        }

        public bool TryGet(string serviceName, out object service, params object[] userParams)
        {
            if(CatApp.CanMake(serviceName))
            {
                service = CatApp.Make(serviceName, userParams);
                return true;
            }
            else
            {
                service = default;
                return false;
            }
        }

        #endregion

        #region 注册服务到容器

        public IBindData Bind<TService, TConcrete>()
            => CatApp.Bind<TService, TConcrete>();

        public IBindData Bind<TService>()
            => CatApp.Bind<TService>();

        public IBindData Bind(string serviceName, Type type, bool isStatic)
            => CatApp.Bind(serviceName, type, isStatic);

        public IServiceContainer BindBuiltinService<TBuiltInService, TService, TConcrete>() where TBuiltInService : IBuiltinServiceBase
        {
            throw new NotImplementedException();
        }

        public IServiceContainer BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : IBuiltinServiceBase
        {
            throw new NotImplementedException();
        }

        public bool BindIf<TService, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public bool BindIf<TService>()
        {
            throw new NotImplementedException();
        }

        public bool BindIf(string serviceName, Type concreate, bool isStatic, out IBindData bindData)
            => CatApp.BindIf(serviceName, concreate, isStatic, out bindData);



        public object Instance(string service, object instance)
            => CatApp.Instance(service, instance);

        public object Instance<TService>(object instance)
            => CatApp.Instance<TService>(instance);

        public IBindData Singleton<TService, TConcrete>()
            => CatApp.Singleton<TService, TConcrete>();

        public IBindData Singleton<TService>()
            => CatApp.Singleton<TService>();


        public bool SingletonIf<TService>(out IBindData bindData)
            => CatApp.SingletonIf<TService>(out bindData);

        public bool SingletonIf<TService, TConcrete>(out IBindData bindData)
            => CatApp.SingletonIf<TService, TConcrete>(out bindData);


        public object Singleton(string service, object instance)
            => CatApp.Instance(service, instance);

        public object Singleton<TService>(object instance)
            => CatApp.Instance<TService>(instance);


        #endregion

        #region 从容器移除服务
        public void Unbind(string serviceName)
        {
            CatApp.Unbind(serviceName);
        }

        public void Unbind<TService>()
        {
            CatApp.Unbind<TService>();
        }

        public void Unbind(Type type)
        {
            CatApp.Unbind(CatApp.GetServiceName(type));
        }

        #endregion

        #region 其他查询
        public string GetServiceName(Type type)
            => CatApp.GetServiceName(type);

        #endregion



        public bool TryGetBuildinService<TBuiltinService>(out TBuiltinService service) where TBuiltinService : IBuiltinServiceBase
        {
            throw new NotImplementedException();
        }

        public bool TryGetBuildinService(Type type, out object service)
        {
            throw new NotImplementedException();
        }

        public bool TryGetBuildinService(string serviceName, out object service)
        {
            throw new NotImplementedException();
        }

        #region 服务注入器的外部支持扩展

        public void RegisterServiceInjector(IServiceInjector injector)
        {
            if(injector == null)
                throw new ArgumentNullException(nameof(injector));
            if(!m_ServiceInjects.Contains(injector))
                m_ServiceInjects.Add(injector);
        }

        public void RemoveServiceInjector(IServiceInjector injector)
        {
            if (injector == null)
                throw new ArgumentNullException(nameof(injector));
            if (m_ServiceInjects.Contains(injector))
                m_ServiceInjects.Remove(injector);
        }

        /// <summary>
        /// 尝试帮助Catlib实现Attribute属性注入
        /// </summary>
        /// <param name="makeServiceBindData"></param>
        /// <param name="makeServiceInstance"></param>
        /// <returns></returns>
        public bool TryServiceAttributeInject(ref Bindable makeServiceBindData, ref object makeServiceInstance)
        {
            if(m_ServiceInjects.Count > 0)
            {
                for(int i = 0; i < m_ServiceInjects.Count; i++)
                {
                    if (m_ServiceInjects[i].TryServiceAttributeInject(ref makeServiceBindData, ref makeServiceInstance, this))
                        return true;
                }
            }
            return false;
        }
        #endregion

    }
}
