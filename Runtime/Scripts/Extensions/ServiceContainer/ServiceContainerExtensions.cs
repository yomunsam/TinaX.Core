using TinaX.Container;

namespace TinaX
{
#nullable enable
    public static class ServiceContainerExtensions
    {
        public static T CreateInstance<T>(this IServiceContainer services, params object?[] userParams)
        {
            return (T)services.CreateInstance(typeof(T), userParams);
        }
    }
#nullable restore
}
