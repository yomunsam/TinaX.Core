using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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

                return _lineColor;
            }
        }

        private static Color _lineColor;

        public static void HorizontalLine(int height , Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, color);
        }

        public static void HorizontalLine(int height = 1)
        {
            HorizontalLine(height, mLineColor);
        }

    }
}

