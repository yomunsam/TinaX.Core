using CatLib;
using CatLib.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Services;

namespace TinaX.Container
{
    public interface IServiceContainer

    {
        Application CatApplication { get; }


        #region Get Services

        /// <summary>
        /// Get Service |  获取服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="userParams"></param>
        /// <returns></returns>
        TService Get<TService>(params object[] userParams);

        /// <summary>
        /// Get Service | 获取服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        object Get(Type type, params object[] userParams);
        object Get(string serviceName, params object[] userParams);

        bool TryGet<TService>(out TService service, params object[] userParams);
        bool TryGet(Type type, out object service, params object[] userParams);
        bool TryGet(string serviceName, out object service, params object[] userParams);
        
        bool TryGetBuildInService<TBuiltInService>(out TBuiltInService service) where TBuiltInService : IBuiltInService;
        bool TryGetBuildInService(Type type, out object service);
        bool TryGetBuildInService(string serviceName, out object service);


        #endregion


        #region Bind Services
        IBindData Singleton<TService, TConcrete>();
        IBindData Singleton<TService>();
        bool SingletonIf<TService>(out IBindData bindData);
        bool SingletonIf<TService, TConcrete>(out IBindData bindData);

        IBindData Bind<TService, TConcrete>();
        IBindData Bind<TService>();
        IBindData Bind(string serviceName, Type type, bool isStatic);

        bool BindIf<TService, TConcrete>(out IBindData bindData);
        bool BindIf<TService>(out IBindData bindData);
        bool BindIf(string serviceName, Type concreate, bool isStatic, out IBindData bindData);


        IBindData BindBuiltInService<TBuiltInService, TService, TConcrete>() where TBuiltInService : IBuiltInService;
        IBindData BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : IBuiltInService;

        object Instance(string service, object instance);
        object Instance<TService>(object instance);
        
        #endregion

        #region Unbind Services
        
        void Unbind<TService>();
        void Unbind(Type type);
        void Unbind(string serviceName);

        #endregion


        #region Inject
        /// <summary>
        /// Dependency injection on a given object
        /// 对给定的对象进行依赖注入
        /// </summary>
        /// <param name="target"></param>
        void Inject(object target);


        #endregion

        string Type2ServiceName(Type type);
        string Type2ServiceName<TService>();
        
    }
}
