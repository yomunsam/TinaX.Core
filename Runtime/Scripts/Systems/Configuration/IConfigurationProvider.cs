using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinaX.Systems.Configuration
{
    public interface IConfigurationProvider
    {
        bool TryGetString(string key, out string value);
    }
}


