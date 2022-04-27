#nullable enable
using UnityEngine;

namespace TinaX.Core.Utils
{
    public static class LocalizationUtil
    {
        /// <summary>
        /// 当前语言环境是不是汉语
        /// </summary>
        /// <returns></returns>
        public static bool IsHans()
        {
#if TINAX_FORCE_ENGLISH
            return false; //强制为false
#else
            return Application.systemLanguage == SystemLanguage.Chinese ||
                Application.systemLanguage == SystemLanguage.ChineseSimplified ||
                Application.systemLanguage == SystemLanguage.ChineseTraditional;
#endif
        }

        public static bool IsJapanese()
        {
#if TINAX_FORCE_ENGLISH
            return false; //强制为false
#else
            return Application.systemLanguage == SystemLanguage.Japanese;
#endif
        }
    }
}
