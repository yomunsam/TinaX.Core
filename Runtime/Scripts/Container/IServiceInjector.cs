using CatLib.Container;
using TinaX.Container;

namespace TinaX.Core.Container
{
    /// <summary>
    /// 服务注入实现
    /// </summary>
    public interface IServiceInjector
    {
        /// <summary>
        /// 尝试帮助服务容器（CatLib）内部实现堆属性的注入
        /// </summary>
        /// <param name="makeServiceBindData"></param>
        /// <param name="makeServiceInstance"></param>
        /// <returns></returns>
        bool TryServiceAttributeInject(ref Bindable makeServiceBindData, ref object makeServiceInstance, IServiceContainer serviceContainer);
    }
}
