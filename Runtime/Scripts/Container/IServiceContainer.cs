using System;
using CatLib.Container;
using TinaX.Container.Internal;
using TinaX.Services.Builtin.Base;

namespace TinaX.Container
{
    /// <summary>
    /// Service container
    /// 服务容器
    /// </summary>
    public interface IServiceContainer : IGetServices
    {
        #region Get Buildin Services

        bool TryGetBuildinService<TBuiltinService>(out TBuiltinService service) where TBuiltinService : IBuiltinServiceBase;
        bool TryGetBuildinService(Type type, out object service);
        bool TryGetBuildinService(string serviceName, out object service);

        #endregion


        #region Add Services
        IBindData Singleton<TService, TConcrete>();
        IBindData Singleton<TService>();
        bool SingletonIf<TService>();
        bool SingletonIf<TService, TConcrete>();

        object Singleton(string service, object instance);
        object Singleton<TService>(object instance);

        IBindData Bind<TService, TConcrete>();
        IBindData Bind<TService>();
        IBindData Bind(string serviceName, Type type, bool isStatic);

        bool BindIf<TService, TConcrete>();
        bool BindIf<TService>();
        bool BindIf(string serviceName, Type concreate, bool isStatic);


        
        [Obsolete("Use Singleton(string, object)")]
        object Instance(string service, object instance);

        [Obsolete("Use Singleton<TService>(instance)")]
        object Instance<TService>(object instance);



        #endregion

        IServiceContainer BindBuiltinService<TBuiltInService, TService, TConcrete>() where TBuiltInService : IBuiltinServiceBase;
        IServiceContainer BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : IBuiltinServiceBase;

        void Unbind(string serviceName);
        void Unbind<TService>();
        void Unbind(Type type);
        
    }
}
