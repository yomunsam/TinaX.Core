using System;

namespace TinaX.Core.ReflectionProvider
{
#nullable enable

    /// <summary>
    /// 反射提供者
    /// </summary>
    public interface IReflectionProvider
    {
        /// <summary>
        /// 尝试获取Type
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="type"></param>
        /// <returns>如果这个属性不归我们管，返回false</returns>
        bool TryGetType(ref object sourceObject, out Type? type);
    }

#nullable restore
}
