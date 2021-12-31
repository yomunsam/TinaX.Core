using System;
using CatLib.Container;
using TinaX.Container.Internal;

namespace TinaX.Container
{
#nullable enable
    /// <summary>
    /// Service container
    /// 服务容器
    /// </summary>
    public interface IServiceContainer : IGetServices
    {
        IXCore XCore { get; }


        #region Add Services
        IBindData Singleton<TService, TConcrete>()
            where TService : class
            where TConcrete : class, TService;

        IBindData Singleton<TService>();

        bool SingletonIf<TService>(out IBindData bindData);
        bool SingletonIf<TService, TConcrete>(out IBindData bindData)
            where TService : class
            where TConcrete: class, TService;

        object Singleton(string service, object instance);
        object Singleton<TService>(object instance);

        IBindData Bind<TService, TConcrete>()
            where TService : class
            where TConcrete : class, TService;

        IBindData Bind<TService>();
        IBindData Bind(string serviceName, Type type, bool isStatic);

        bool BindIf<TService, TConcrete>(out IBindData bindData)
            where TService : class
            where TConcrete : class, TService;

        bool BindIf<TService>(out IBindData bindData);
        bool BindIf(string serviceName, Type concreate, bool isStatic, out IBindData bindData);


        
        [Obsolete("Use Singleton(string, object)")]
        object Instance(string service, object instance);

        [Obsolete("Use Singleton<TService>(instance)")]
        object Instance<TService>(object instance);

        #endregion

        void Unbind(string serviceName);
        void Unbind<TService>();
        void Unbind(Type type);

        /// <summary>
        /// 可否获取服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        bool CanGet(string serviceName);

        /// <summary>
        /// 可否获取服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        bool CanGet<TService>() where TService : class;

        #region 周边功能
        object CreateInstance(Type type, params object?[] userParams);
        void Inject(object sourceObject);
        
        #endregion


    }
#nullable restore
}
