using System.Linq;
using TinaX.Services;
using UnityEngine;

namespace TinaX.Core.Extensions
{
    public static class LocalizationExtensions
    {
        public static bool IsHans(this IXCore core)
        {
            if(core.Services.TryGet<ILocalizationService>(out var localization))
            {
                var langs = localization.GetCurrentLanguages();
                if (langs != null)
                {
                    return langs.Any(lang => lang == UnityEngine.SystemLanguage.Chinese || lang == UnityEngine.SystemLanguage.ChineseSimplified || lang == UnityEngine.SystemLanguage.ChineseTraditional);
                }
            }

            return Application.systemLanguage == SystemLanguage.Chinese ||
                Application.systemLanguage == SystemLanguage.ChineseSimplified ||
                Application.systemLanguage == SystemLanguage.ChineseTraditional;
        }
    }
}
