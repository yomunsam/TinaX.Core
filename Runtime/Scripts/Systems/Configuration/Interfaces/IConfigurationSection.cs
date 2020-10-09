using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Configuration
{
    public interface IConfigurationSection : IConfiguration
    {
        string Key { get; }
        string Path { get; }
        string Value { get; }

    }
}
