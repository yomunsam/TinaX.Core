using System;

namespace TinaX.Core.Activator
{
    public interface ICreateInstance
    {
        string ProviderName { get; }
        bool TryCreateInstance(Type type, out object instance, params object[] args);
    }
}
