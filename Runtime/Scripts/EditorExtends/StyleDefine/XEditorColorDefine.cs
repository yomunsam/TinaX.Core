/*
 * 该文档是对编辑器风格的定义，如编辑器下的文本颜色 等
 * 之所以放在TinaX.Core下而非Editor下，是因为我们需要在Console输出等地方使用到
 * 
 * 
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TinaX.Internal
{
    public static class XEditorColorDefine
    {
        /// <summary>
        /// 通常颜色
        /// </summary>
        public static Color Color_Normal
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return Color.white;
                else
                    return Color.black;

#else
                return Color.black;
#endif
            }
        }
    }
}
