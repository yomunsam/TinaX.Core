using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Const;
using CatLib;
using CatLib.Container;
using Application = CatLib.Application;
using System.Reflection;
using TinaX.Internal;
using TinaX.Services;

namespace TinaX.Container
{
    public class ServiceContainer : IServiceContainer
    {
        public Application CatApplication { get; private set; }

        private XCore m_Core;

        public ServiceContainer(XCore core)
        {
            m_Core = core;
            CatApplication = new XCatApplication(m_Core);
            App.That = CatApplication;
        }

        #region 构建和获取服务

        public TService Get<TService>(params object[] userParams) => CatApplication.Make<TService>(userParams);

        public object Get(Type type, params object[] userParams)
        {
            string service_name = CatApplication.Type2Service(type);
            return CatApplication.Make(service_name, userParams);
        }
        public object Get(string serviceName, params object[] userParams)
            => CatApplication.Make(serviceName, userParams);

        public bool TryGet<TService>(out TService service, params object[] userParams)
        {
            if (CatApplication.IsStatic<TService>() 
                || CatApplication.IsAlias<TService>()
                || CatApplication.HasBind<TService>()
                || CatApplication.HasInstance<TService>())
            {
                service = CatApplication.Make<TService>(userParams);
                return true;
            }
            
            service = default;
            return false;
        }

        public bool TryGet(Type type, out object service, params object[] userParams)
        {
            string service_name = CatApplication.Type2Service(type);
            if (CatApplication.IsStatic(service_name) 
                || CatApplication.IsAlias(service_name)
                || CatApplication.HasBind(service_name)
                || CatApplication.HasInstance(service_name))
            {
                service = CatApplication.Make(service_name, userParams);
                return true;
            }

            service = default;
            return false;
        }

        public bool TryGet(string service_name, out object service, params object[] userParams)
        {
            if (CatApplication.IsStatic(service_name)
                || CatApplication.IsAlias(service_name)
                || CatApplication.HasBind(service_name)
                || CatApplication.HasInstance(service_name))
            {
                service = CatApplication.Make(service_name, userParams);
                return true;
            }

            service = default;
            return false;
        }

        public bool TryGetBuildInService<TBuiltInService>(out TBuiltInService service) where TBuiltInService : TinaX.Services.IBuiltInService
        {
            if (CatApplication.IsAlias<TBuiltInService>()
                || CatApplication.HasBind<TBuiltInService>()
                || CatApplication.HasInstance<TBuiltInService>()
                || CatApplication.IsStatic<TBuiltInService>())
            {
                service = CatApplication.Make<TBuiltInService>();
                return true;
            }
            service = default;
            return false;
        }

        public bool TryGetBuildInService(Type type , out object service)
        {
            if(!type.IsAssignableFrom(typeof(TinaX.Services.IBuiltInService)))
            {
                service = default;
                return false;
            }

            string service_name = CatApplication.Type2Service(type);
            if (CatApplication.IsStatic(service_name)
                || CatApplication.IsAlias(service_name)
                || CatApplication.HasBind(service_name)
                || CatApplication.HasInstance(service_name))
            {
                service = CatApplication.Make(service_name);
                return true;
            }

            service = default;
            return false;
        }

        public bool TryGetBuildInService(string service_name, out object service)
        {
            if (CatApplication.IsStatic(service_name)
                || CatApplication.IsAlias(service_name)
                || CatApplication.HasBind(service_name)
                || CatApplication.HasInstance(service_name))
            {
                service = CatApplication.Make(service_name);
                return true;
            }

            service = default;
            return false;
        }

        #endregion

        #region 注册绑定服务

        /// <summary>
        /// 绑定实例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <returns></returns>
        public IBindData Bind<TService, TConcrete>()
            => CatApplication.Bind<TService, TConcrete>();

        public IBindData Bind(string serviceName, Type type, bool isStatic)
            => CatApplication.Bind(serviceName, type, isStatic);

        public IBindData Bind<TService>()
            => CatApplication.Bind<TService>();

        public bool BindIf<TService, TConcrete>(out IBindData bindData)
            => CatApplication.BindIf<TService, TConcrete>(out bindData);

        public bool BindIf<TService>(out IBindData bindData)
            => CatApplication.BindIf<TService>(out bindData);

        public bool BindIf(string serviceName, Type concreate, bool isStatic, out IBindData bindData)
            => CatApplication.BindIf(serviceName, concreate, isStatic, out bindData);

        /// <summary>
        /// 绑定单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <returns></returns>
        public IBindData Singleton<TService, TConcrete>()
            => CatApplication.Singleton<TService, TConcrete>();

        public IBindData Singleton<TService>()
            => CatApplication.Singleton<TService>();

        public bool SingletonIf<TService, TConcrete>(out IBindData bindData)
            => CatApplication.SingletonIf<TService, TConcrete>(out bindData);

        public bool SingletonIf<TService>(out IBindData bindData)
            => CatApplication.SingletonIf<TService>(out bindData);

        /// <summary>
        /// 实现内置服务接口
        /// </summary>
        /// <typeparam name="TBuiltInService"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <returns></returns>
        public IBindData BindBuiltInService<TBuiltInService, TService, TConcrete>() where TBuiltInService : TinaX.Services.IBuiltInService
        {
            m_Core.m_TryGetIAppDomain = false;
            return CatApplication.Singleton<TService, TConcrete>()
                .Alias<TBuiltInService>();
        }

        public IBindData BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : TinaX.Services.IBuiltInService
        {
            m_Core.m_TryGetIAppDomain = false;
            return CatApplication.Singleton<TBuiltInService, TConcrete>();
        }


        #endregion

        #region 解绑

        public void Unbind<TService>()
        {
            CatApplication.Unbind<TService>();
        }

        public void Unbind(Type type)
        {
            CatApplication.Unbind(CatApplication.Type2Service(type));
        }

        public void Unbind(string serviceName)
        {
            CatApplication.Unbind(serviceName);
        }

        #endregion

        #region 注入

        public void Inject(object target)
        {
            Type obj_type = target.GetType();
            foreach (var field in obj_type.GetRuntimeFields())
            {
                var attr = field.GetCustomAttribute<InjectAttribute>(true);
                if (attr == null)
                    continue;
                if (this.TryGet(field.FieldType, out var _service))
                {
                    field.SetValue(target, _service);
                    continue;
                }

                if (attr.Nullable)
                    continue;
                else
                    throw new ServiceNotFoundException(field.FieldType); //抛异常
            }

            foreach (var property in obj_type.GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<InjectAttribute>(true);
                if (attr == null)
                    continue;
                if (this.TryGet(property.PropertyType,out var _service))
                {
                    property.SetValue(target, _service);
                    continue;
                }

                if (attr.Nullable)
                    continue;
                else
                    throw new ServiceNotFoundException(property.PropertyType); //抛异常
            }
        }

        #endregion


        public string Type2ServiceName(Type type) => CatApplication.Type2Service(type);
        public string Type2ServiceName<TService>() => CatApplication.Type2Service<TService>();
    }
}
