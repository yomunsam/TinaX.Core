using System.Collections.Generic;
using System.Threading.Tasks;

namespace TinaX.Systems.Configuration
{

    /// <summary>
    /// Represents a type used to build application configuration.
    /// </summary>
    public interface IConfigurationBuilder
    {
        IList<IConfigurationSource> Sources { get; }


        /// <summary>
        /// Adds a new configuration source.
        /// 添加配置来源
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IConfigurationBuilder Add(IConfigurationSource source);

        Task<IConfigurationRoot> BuildAsync();
    }
}
