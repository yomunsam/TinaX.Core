using System;
using TinaX.Container.Internal;
using TinaX.Services.Builtin;

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
        IServiceContainer Singleton<TService, TConcrete>();
        IServiceContainer Singleton<TService>();
        bool SingletonIf<TService>();
        bool SingletonIf<TService, TConcrete>();

        IServiceContainer Bind<TService, TConcrete>();
        IServiceContainer Bind<TService>();
        IServiceContainer Bind(string serviceName, Type type, bool isStatic);

        bool BindIf<TService, TConcrete>();
        bool BindIf<TService>();
        bool BindIf(string serviceName, Type concreate, bool isStatic);


        
        [Obsolete]
        object Instance(string service, object instance);

        [Obsolete]
        object Instance<TService>(object instance);



        #endregion

        IServiceContainer BindBuiltinService<TBuiltInService, TService, TConcrete>() where TBuiltInService : IBuiltinServiceBase;
        IServiceContainer BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : IBuiltinServiceBase;
    }
}
