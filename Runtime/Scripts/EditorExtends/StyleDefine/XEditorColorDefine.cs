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
        /// 纯色（纯白、纯黑）
        /// </summary>
        public static Color Color_Normal_Pure
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

        /// <summary>
        /// 通常色
        /// </summary>
        public static Color Color_Normal
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return new Color(
                        196f / 255f, 
                        196f / 255f, 
                        196f / 255f, 1);
                else
                    return new Color(
                        55f / 255f, 
                        55f / 255f, 
                        55f / 255f, 1);

#else
                return new Color(196f / 255f, 196f / 255f, 196f / 255f, 1);
#endif
            }
        }

        /// <summary>
        /// 警告色
        /// </summary>
        public static Color Color_Warning
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return new Color(1, 160f / 255f, 7f / 255f, 1);
                else
                    return new Color(
                        226f/255f, 
                        96f / 255f, 
                        0, 1);

#else
                return new Color(1, 160f / 255f, 7f / 255f, 1);
#endif
            }
        }

        public static Color Color_Error
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return new Color(1, 89f / 255f, 90f / 255f, 1);
                else
                    return new Color(1, 64f / 255f, 57f / 255f, 1);

#else
                return new Color(1, 89f / 255f, 90f / 255f, 1);
#endif
            }
        }

        /// <summary>
        /// 安全色
        /// </summary>
        public static Color Color_Safe
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return new Color(101f / 255f, 201f / 255f, 0, 1);
                else
                    return new Color(18f / 255f, 144f / 255f, 0, 1);

#else
                return new Color(101 / 255f, 201 / 255f, 0, 1);
#endif
            }
        }

        /// <summary>
        /// 着重色
        /// </summary>
        public static Color Color_Emphasize
        {
            get
            {
#if UNITY_EDITOR
                if (EditorGUIUtility.isProSkin)
                    return new Color(44f / 255f, 232f / 255f, 1, 1);
                else
                    return new Color(0, 177f / 255f, 191 / 255, 1);

#else
                return new Color(44f / 255f, 232f / 255f, 1, 1);
#endif
            }
        }

    }
}
