using System.Linq;
using TinaX.Services;

namespace TinaX.Core.Localization
{
    public static class IXCoreLocalizationExtend
    {
        public static bool IsCmnHans(this IXCore core)
        {
            if (core.Services.TryGet<ILocalizationService>(out var locatization))
            {
                var langs = locatization.GetCurrentLanguages();
                if (langs == null) return false;
                return langs.Any(l => l == UnityEngine.SystemLanguage.Chinese || l == UnityEngine.SystemLanguage.ChineseSimplified);
            }
            else
                return false;
        }
    }
}
