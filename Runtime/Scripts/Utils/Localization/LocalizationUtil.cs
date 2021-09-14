using UnityEngine;

namespace TinaX.Core.Utils
{
    public static class LocalizationUtil
    {
        public static bool IsHans()
            => Application.systemLanguage == SystemLanguage.Chinese ||
                Application.systemLanguage == SystemLanguage.ChineseSimplified ||
                Application.systemLanguage == SystemLanguage.ChineseTraditional;
    }
}
