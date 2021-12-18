using UnityEngine;

namespace TinaXEditor.Core.Utils.Localization
{
    public static class EditorLocalizationUtil
    {
        /// <summary>
        /// 当前编辑器是不是汉语
        /// </summary>
        /// <returns></returns>
        public static bool IsHans()
        {
#if TINAX_EDITOR_ENGLISH
            return false; //强制为false
#else
            return Application.systemLanguage == SystemLanguage.Chinese ||
                Application.systemLanguage == SystemLanguage.ChineseSimplified ||
                Application.systemLanguage == SystemLanguage.ChineseTraditional;
#endif
        }

        public static bool IsJapanese()
        {
#if TINAX_EDITOR_ENGLISH
            return false; //强制为false
#else
            return Application.systemLanguage == SystemLanguage.Japanese;
#endif
        }
    }
}
