using System.Collections.Generic;

namespace TinaX.Systems.Configuration
{
    public interface IConfigurationRoot : IConfiguration
    {
        IEnumerable<IConfigurationProvider> Providers { get; }
    }
}
