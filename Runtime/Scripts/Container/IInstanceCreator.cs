using System;

namespace TinaX.Core.Container
{
    /// <summary>
    /// 实例创建器 接口
    /// </summary>
    public interface IInstanceCreator
    {
        public bool TryCreateInstance(Type type, out object result, object[] args);
    }
}
