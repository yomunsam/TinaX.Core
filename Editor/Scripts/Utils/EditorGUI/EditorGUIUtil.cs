using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using TinaXEditor.Internal.Utils;

namespace TinaXEditor.Utils
{
    public static class EditorGUIUtil
    {

        static GUIStyle _horizontalLine;
        static GUIStyle horizontalLine
        {
            get
            {
                if (_horizontalLine == null)
                {
                    _horizontalLine = new GUIStyle();
                    _horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
                    _horizontalLine.margin = new RectOffset(0, 0, 4, 4);
                    _horizontalLine.fixedHeight = 1;
                }
                return _horizontalLine;
            }
        }
        
        private static Color mLineColor
        {
            get
            {
                if (_lineColor == null)
                    _lineColor = TinaX.Internal.XEditorColorDefine.Color_Normal;

                return _lineColor.Value;
            }
        }

        private static Color? _lineColor;

        /// <summary>
        /// 横线
        /// </summary>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void HorizontalLine(int height , Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// 横线
        /// </summary>
        /// <param name="height"></param>
        public static void HorizontalLine(int height = 1)
        {
            HorizontalLine(height, mLineColor);
        }

        /// <summary>
        /// prompt | 输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="callback"></param>
        /// <param name="defaultContent"></param>
        public static void Prompt(string title, Action<bool,string> callback, string defaultContent = null)
        {
            Prompt(callback, title: title, defaultContent: defaultContent);
        }

        public static void Prompt(Action<bool, string> callback, string title = "Message:", string message = ">", string defaultContent = null, string comfirn_btn_text = "Confirm", string cancel_btn_text = null)
        {
            if (PromptIMGUI.IsActive) return;
            ScriptableSingleton<PromptParam>.instance.ClearData();
            ScriptableSingleton<PromptParam>.instance.callback = callback;
            ScriptableSingleton<PromptParam>.instance.defaultContent = defaultContent;
            ScriptableSingleton<PromptParam>.instance.messageContent = message;
            ScriptableSingleton<PromptParam>.instance.title = new GUIContent(title);
            ScriptableSingleton<PromptParam>.instance.confirm_text = comfirn_btn_text;
            ScriptableSingleton<PromptParam>.instance.cancel_text = cancel_btn_text;

            PromptIMGUI.OpenUI();
        }


        private static bool? _isHans;

        /// <summary>
        /// 系统是否为国语汉语
        /// cmn - 国语（普通话）
        /// hans - 汉语
        /// </summary>
        public static bool IsCmnHans
        {
            get
            {
                if (_isHans == null)
                {
                    _isHans = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified);
                }
                return _isHans.Value;
            }
        }

        private static bool? _isJapanese;

        /// <summary>
        /// システムが日本語かどうか
        /// </summary>
        public static bool IsJapanese
        {
            get
            {
                if (_isJapanese == null)
                    _isJapanese = (Application.systemLanguage == SystemLanguage.Japanese);
                return _isJapanese.Value;
            }
        }

    }
}

