using System;
using System.Collections.Generic;
using System.Linq;

namespace TinaX.Systems.Configuration.Internal
{
    internal static class InternalConfigurationRootExtensions
    {

        /// <summary>
        /// Gets the immediate children sub-sections of configuration root based on key.
        /// 根据key获取path的直接子级
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static IEnumerable<IConfigurationSection> GetChildrenImplementation(this IConfigurationRoot root, string path)
        {
            return root.Providers
                .Aggregate(Enumerable.Empty<string>(),
                    (seed, source) => source.GetChildKeys(seed, path))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(key => root.GetSection(path == null ? key : ConfigurationPath.Combine(path, key)));
        }
    }
}
