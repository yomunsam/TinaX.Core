using System;
using System.Collections.Generic;
using System.Reflection;
using CatLib.Container;
using TinaX.Core.Container;

namespace TinaX.Container
{
#nullable enable
    public class ServiceContainer : IServiceContainer
    {
        private readonly XCore m_Core;
        private readonly List<IReflectionProvider> m_TypeProviders = new List<IReflectionProvider>();
        private readonly Type m_InjectAttributeType;

        public ServiceContainer(XCore core)
        {
            m_Core = core;
            CatApp = new XCatApplication(core, this);

            CatApp.Instance<IXCore>(core);
            m_InjectAttributeType = typeof(TinaX.InjectAttribute);
        }

        public XCatApplication CatApp { get;}




        #region 构建获取服务
        public TService Get<TService>(params object?[] userParams)
            => CatApp.Make<TService>(userParams);

        public object Get(Type type, params object?[] userParams)
            => CatApp.Make(type, userParams);

        public object Get(string serviceName, params object?[] userParams)
            => CatApp.Make(serviceName, userParams);

        public bool TryGet<TService>(out TService? service, params object?[] userParams) where TService : class
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

        public bool TryGet(Type type, out object? service, params object?[] userParams)
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

        public bool TryGet(string serviceName, out object? service, params object?[] userParams)
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
            where TService : class
            where TConcrete : class, TService
            => CatApp.Bind<TService, TConcrete>();

        public IBindData Bind<TService>()
            => CatApp.Bind<TService>();

        public IBindData Bind(string serviceName, Type type, bool isStatic)
            => CatApp.Bind(serviceName, type, isStatic);



        public bool BindIf<TService, TConcrete>(out IBindData bindData)
            where TService : class
            where TConcrete : class, TService
            => CatApp.BindIf<TService, TConcrete>(out bindData);

        public bool BindIf<TService>(out IBindData bindData)
            => CatApp.BindIf<TService>(out bindData);

        public bool BindIf(string serviceName, Type concreate, bool isStatic, out IBindData bindData)
            => CatApp.BindIf(serviceName, concreate, isStatic, out bindData);



        public object Instance(string service, object instance)
            => CatApp.Instance(service, instance);

        public object Instance<TService>(object instance)
            => CatApp.Instance<TService>(instance);

        public IBindData Singleton<TService, TConcrete>()
            where TService : class
            where TConcrete : class, TService
            => CatApp.Singleton<TService, TConcrete>();

        public IBindData Singleton<TService>()
            => CatApp.Singleton<TService>();


        public bool SingletonIf<TService>(out IBindData bindData)
            => CatApp.SingletonIf<TService>(out bindData);

        public bool SingletonIf<TService, TConcrete>(out IBindData bindData)
            where TService : class
            where TConcrete : class, TService
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


        #region 周边功能

        /// <summary>
        /// 创建对象（会尝试对构造函数进行依赖注入）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object CreateInstance(Type type, params object?[] userParams)
        {
            string serviceName = GetServiceName(type);
            if(CatApp.BindIf(serviceName, type, false, out var bindData))
            {
                var result = CatApp.Make(serviceName, userParams);
                CatApp.Unbind(bindData);
                return result;
            }
            throw new Exception($"Type \"{type}\" can not create. please check if there is a list of types that cannot be built.");
            /*
             * 这儿的实现思路是把给定的类型临时注册给服务容器，创建完了再从服务容器移除
             * 这样的优点是实现比较统一，包括ILRuntime兼容性啥的都比较好做
             * 此外还有一种实现思路就是在容器外反射构造函数并尝试注入。
             */
        }

        /// <summary>
        /// 对一个已存在的对象进行注入
        /// </summary>
        /// <param name="sourceObject"></param>
        public void Inject(object sourceObject)
        {
            if (!TryGetType(ref sourceObject, out Type? sourceType))
            {
                sourceType = sourceObject.GetType();
            }

            var properties = sourceType!.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (properties != null)
            {
                for(int i = 0; i< properties.Length; i++)
                {
                    PropertyInfo property = properties[i];

                    //需要注入？
                    bool? b_inject = CanInjected(ref property);
                    if(!b_inject.HasValue)
                    {
                        b_inject = property.CanWrite && property.IsDefined(m_InjectAttributeType, true);
                    }

                    if (!b_inject.Value)
                        continue;

                    if (property.PropertyType.IsValueType) //如果是值类型，直接跳过
                        continue;

                    //获取服务，然后注入
                    var serviceName = GetServiceName(property.PropertyType);
                    if(this.TryGet(serviceName, out var serviceInstance))
                    {
                        property.SetValue(sourceObject, serviceInstance);
                        continue;
                    }

                    bool? b_skip = CanInjectedSkip(ref property);
                    if(!b_skip.HasValue)
                    {
                        var attribute = property.GetCustomAttribute<TinaX.InjectAttribute>();
                        if (attribute == null || attribute.Nullable)
                            continue;
                    }

                    //错误
                    throw new Exception($"Service not found {property.PropertyType}"); //抛异常
                }
            }
        } 

        #endregion

        #region 服务注入器的外部支持扩展


        /// <summary>
        /// 注册类型提供者
        /// </summary>
        /// <param name="provider"></param>
        public void RegisterReflectionProvider(IReflectionProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (!m_TypeProviders.Contains(provider))
                m_TypeProviders.Add(provider);
        }


        public void RemoveReflectionProvider(IReflectionProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (m_TypeProviders.Contains(provider))
                m_TypeProviders.Remove(provider);
        }
        
        /// <summary>
        /// CatLib Application调用这个方法来尝试获取类型
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TryGetType(ref object sourceObject, out Type? type)
        {
            for(int i = 0; i < m_TypeProviders.Count; i++)
            {
                if (m_TypeProviders[i].TryGetType(ref sourceObject, out type))
                    return true;
            }

            type = default;
            return false;
        }

        public bool? CanInjected(ref PropertyInfo property)
        {
            for (int i = 0; i < m_TypeProviders.Count; i++)
            {
                var b = m_TypeProviders[i].CanInjected(ref property);
                if (b.HasValue)
                    return b;
            }
            return null;
        }

        public bool? CanInjectedSkip(ref PropertyInfo property)
        {
            for (int i = 0; i < m_TypeProviders.Count; i++)
            {
                var b = m_TypeProviders[i].CanInjectedSkip(ref property);
                if (b.HasValue)
                    return b;
            }
            return null;
        }

        #endregion

    }
#nullable restore
}
