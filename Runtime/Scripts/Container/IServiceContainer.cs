using System;
using CatLib.Container;
using TinaX.Container.Internal;
using TinaX.Core.Container;

namespace TinaX.Container
{
#nullable enable
    /// <summary>
    /// Service container
    /// 服务容器
    /// </summary>
    public interface IServiceContainer : IGetServices
    {



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

        #region 周边功能
        object CreateInstance(Type type, params object?[] userParams);
        void Inject(object sourceObject);
        #endregion

        #region 高级功能
        void RegisterReflectionProvider(IReflectionProvider provider);
        void RemoveReflectionProvider(IReflectionProvider provider);
        
        #endregion
    }
#nullable restore
}
