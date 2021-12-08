/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 * 
 * 日志颜色助手
 * 输出日志到Unity控制台的时候，可以指定日志颜色。
 * 本Helper类定义了一些规范色，以便输出使用
 * 
 * 根据皮肤调整颜色：
 * Unity的编辑器有黑白两套Theme，如果同样输出一种颜色，可能在某些Theme上看不清晰, 
 * 因此这里会判断编辑器主题的颜色，并调整输出Log的颜色
 * 
 * C# 定义符号：
 * 在Runtime环境下显示日志时可能也需要调整颜色，比如SRDebug这个插件的UI背景就是黑色的
 * 所以我们需要定义一些宏来在Runtime下调整颜色
 * TINAX_RUNTIME_LOG_DARK : 指定输出的Log颜色适合在深色背景下展示
 * TINAX_RUNTIME_LOG_NOCOLOR : 取消颜色
 */


using UnityEditor;
using UnityEngine;

namespace TinaX.Core.Helper.LogColor
{
    public static class LogColorHelper
    {
        private static readonly Color _Color_Normal_In_Dark;
        private static readonly Color _Color_Normal_In_Light;
        private static readonly string _Color_Normal_In_Dark_16;
        private static readonly string _Color_Normal_In_Light_16;

        private static readonly Color _Color_Primary_In_Dark;
        private static readonly Color _Color_Primary_In_Light;
        private static readonly string _Color_Primary_In_Dark_16;
        private static readonly string _Color_Primary_In_Light_16;


        static LogColorHelper()
        {
            _Color_Normal_In_Dark = new Color(196f / 255f, 196f / 255f, 196f / 255f, 1);
            _Color_Normal_In_Light = new Color(55f / 255f, 55f / 255f, 55f / 255f, 1);
            _Color_Normal_In_Dark_16 = ColorUtility.ToHtmlStringRGBA(_Color_Normal_In_Dark);
            _Color_Normal_In_Light_16 = ColorUtility.ToHtmlStringRGBA(_Color_Normal_In_Light);

            _Color_Primary_In_Dark = new Color(48f / 255f, 128f / 255f, 206f / 255f, 1);
            _Color_Primary_In_Light = new Color(42f / 255f, 110f / 255f, 192f / 255f, 1);
            _Color_Primary_In_Dark_16 = ColorUtility.ToHtmlStringRGBA(_Color_Primary_In_Dark);
            _Color_Primary_In_Light_16 = ColorUtility.ToHtmlStringRGBA(_Color_Primary_In_Light);
        }

        /// <summary>
        /// 通常色
        /// </summary>
        public static Color Color_Normal
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? _Color_Normal_In_Dark : _Color_Normal_In_Light;
#elif !UNITY_EDITOR && TINAX_RUNTIME_LOG_DARK
                return _Color_Normal_In_Dark;
#else
                return _Color_Normal_In_Light;
#endif
            }
        }

        /// <summary>
        /// 主要色
        /// </summary>
        public static Color Color_Primary
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? _Color_Primary_In_Dark : _Color_Primary_In_Light;
#elif !UNITY_EDITOR && TINAX_RUNTIME_LOG_DARK
                return _Color_Primary_In_Dark;
#else
                return _Color_Primary_In_Light;
#endif
            }
        }

        /// <summary>
        /// 通常色（16进制）
        /// </summary>
        public static string Color_Normal_16
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? _Color_Normal_In_Dark_16 : _Color_Normal_In_Light_16;
#elif !UNITY_EDITOR && TINAX_RUNTIME_LOG_DARK
                return _Color_Normal_In_Dark_16;
#else
                return _Color_Normal_In_Light_16;
#endif
            }
        }

        /// <summary>
        /// 主要色（16进制）
        /// </summary>
        public static string Color_Primary_16
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? _Color_Primary_In_Dark_16 : _Color_Primary_In_Light_16;
#elif !UNITY_EDITOR && TINAX_RUNTIME_LOG_DARK
                return _Color_Primary_In_Dark_16;
#else
                return _Color_Primary_In_Light_16;
#endif
            }
        }


        public static string NormalLog(string source)
        {
#if !UNITY_EDITOR && TINAX_RUNTIME_LOG_NOCOLOR
            //不处理颜色
            return source;
#else
            return $"<color=#{Color_Normal_16}>{source}</color>";
#endif
        }

        public static string PrimaryLog(string source)
        {
#if !UNITY_EDITOR && TINAX_RUNTIME_LOG_NOCOLOR
            //不处理颜色
            return source;
#else
            return $"<color=#{Color_Primary_16}>{source}</color>";
#endif
        }


    }
}
