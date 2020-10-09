using System.Collections.Generic;

namespace TinaX.Systems.Configuration
{
    public interface IConfiguration
    {
        string this[string key] { get; set; }
        //IConfiguration AddConfigurationProvider(IConfigurationProvider provider);
        //干掉，改用与微软一致的实现方式

        IEnumerable<IConfigurationSection> GetChildren();
        IConfigurationSection GetSection(string key);
    }
}
