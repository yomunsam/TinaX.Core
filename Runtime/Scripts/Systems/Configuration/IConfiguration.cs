using System.Collections.Generic;

namespace TinaX.Systems.Configuration
{
    public interface IConfiguration
    {
        string this[string key] { get; }
        IConfiguration AddConfigurationProvider(IConfigurationProvider provider);
        IEnumerable<IConfigurationSection> GetChildren();
        IConfigurationSection GetSection(string key);
    }
}
