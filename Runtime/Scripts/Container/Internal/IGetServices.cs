using System;

namespace TinaX.Container.Internal
{
    public interface IGetServices
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
        object Get(string serviceName, params object[] userParams);

        bool TryGet<TService>(out TService service, params object[] userParams);
        bool TryGet(Type type, out object service, params object[] userParams);
        bool TryGet(string serviceName, out object service, params object[] userParams);

        #endregion

        string GetServiceName(Type type);
    }
}
