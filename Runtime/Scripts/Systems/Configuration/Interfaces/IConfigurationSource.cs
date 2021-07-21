using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Configuration
{
    /// <summary>
    /// Represents a source of configuration key/values for an application.
    /// 配置键/值的来源。
    /// </summary>
    public interface IConfigurationSource
    {
        Task<IConfigurationProvider> BuildAsync(IConfigurationBuilder builder);
    }
}
