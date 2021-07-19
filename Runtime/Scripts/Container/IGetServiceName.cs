using System;

namespace TinaX.Container
{
    public interface IGetServiceName
    {
        bool TryGetServiceName(Type serviceType, out string serviceName);
    }
}
