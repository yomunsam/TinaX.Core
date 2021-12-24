using System;
using System.Reflection;

namespace TinaX.Core.Container
{
#nullable enable
    /// <summary>
    /// 类型提供者，帮助服务容器处理反射
    /// </summary>
    public interface IReflectionProvider
    {
        /// <summary>
        /// 检查给定的属性能否被依赖注入
        /// </summary>
        /// <param name="property"></param>
        /// <param name="required">是否必须的</param>
        /// <returns>如果这个属性我们提供者管不着，就返回null</returns>
        bool? CanInjected(ref PropertyInfo property);

        /// <summary>
        /// 检查给定的依赖注入属性可否跳过（留空）
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool? CanInjectedSkip(ref PropertyInfo property);

        /// <summary>
        /// 尝试获取Type
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="type"></param>
        /// <returns>如果这个属性不归我们管，返回false</returns>
        bool TryGetType(ref object sourceObject, out Type type);
    }
#nullable restore
}
