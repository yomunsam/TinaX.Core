using System;
using CatLib;
using CatLib.Container;
using TinaX.Container;
using TinaX.Services.Builtin;
using Application = CatLib.Application;

namespace TinaX.Catlib
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly XCore m_Core;

        public ServiceContainer(XCore core)
        {
            m_Core = core;
            CatApp = new XCatApplication();

            CatApp.Instance<IXCore>(core);
        }

        public Application CatApp { get;}




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

        public IServiceContainer Bind<TService, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public IServiceContainer Bind<TService>()
        {
            throw new NotImplementedException();
        }

        public IServiceContainer Bind(string serviceName, Type type, bool isStatic)
        {
            throw new NotImplementedException();
        }

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

        public bool BindIf(string serviceName, Type concreate, bool isStatic)
        {
            throw new NotImplementedException();
        }

        

        public object Instance(string service, object instance)
        {
            throw new NotImplementedException();
        }

        public object Instance<TService>(object instance)
        {
            throw new NotImplementedException();
        }

        public IServiceContainer Singleton<TService, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public IServiceContainer Singleton<TService>()
        {
            throw new NotImplementedException();
        }

        public bool SingletonIf<TService>()
        {
            throw new NotImplementedException();
        }

        public bool SingletonIf<TService, TConcrete>()
        {
            throw new NotImplementedException();
        }

        

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

    }
}
