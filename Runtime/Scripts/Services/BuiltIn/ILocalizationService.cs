using System.Collections.Generic;
using UnityEngine;

namespace TinaX.Services
{
    public interface ILocalizationService
    {
        IEnumerable<SystemLanguage> GetCurrentLanguages();

        string GetText(string key, string group, string defaultValue = null);
    }
}
