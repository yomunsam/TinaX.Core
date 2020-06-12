using CatLib.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.Services;

namespace TinaX.Const
{
    public interface IServiceContainer
    {
        

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
        
        bool TryGet<TService>(out TService service, params object[] userParams);
        bool TryGet(Type type, out object service, params object[] userParams);
        
        bool TryGetBuildInService<TBuiltInService>(out TBuiltInService service) where TBuiltInService : IBuiltInService;
        bool TryGetBuildInService(Type type, out object service);


        #endregion


        #region Bind Services
        IBindData Singleton<TService, TConcrete>();
        IBindData Singleton<TService>();
        bool SingletonIf<TService>(out IBindData bindData);
        bool SingletonIf<TService, TConcrete>(out IBindData bindData);

        IBindData Bind<TService, TConcrete>();
        bool BindIf<TService, TConcrete>(out IBindData bindData);
        IBindData Bind<TService>();
        bool BindIf<TService>(out IBindData bindData);


        IBindData BindBuiltInService<TBuiltInService, TService, TConcrete>() where TBuiltInService : IBuiltInService;
        IBindData BindBuiltInService<TBuiltInService, TConcrete>() where TBuiltInService : IBuiltInService;

        #endregion

        #region Unbind Services
        
        void Unbind<TService>();
        void Unbind(Type type);
        #endregion


        #region Inject
        /// <summary>
        /// Dependency injection on a given object
        /// 对给定的对象进行依赖注入
        /// </summary>
        /// <param name="target"></param>
        void Inject(object target);
        

        #endregion
    }
}
