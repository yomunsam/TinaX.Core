using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using TinaX;

namespace TinaXEditor.Internal.Utils
{
    internal class PromptIMGUI : EditorWindow
    {

        private static PromptIMGUI wnd;

        public static bool IsActive => wnd != null;

        

        public static void OpenUI()
        {
            if(wnd == null)
            {
                wnd = GetWindow<PromptIMGUI>(true);
                if (ScriptableSingleton<PromptParam>.instance.title == null)
                    wnd.titleContent = new GUIContent("Message:");
                else
                    wnd.titleContent = ScriptableSingleton<PromptParam>.instance.title;

                //var rect = wnd.position;
                //rect.width = 400;
                //rect.height = 100;
                wnd.minSize = new Vector2(399, 99);
                //wnd.position = rect;
                wnd.maxSize = new Vector2(400, 150);
            }
        }


        private GUIStyle _style_msg;
        private GUIStyle style_msg
        {
            get
            {
                if (_style_msg == null)
                {
                    _style_msg = new GUIStyle(EditorStyles.largeLabel);
                    //_style_msg.clipping = TextClipping.Clip;
                    _style_msg.wordWrap = true;
                }
                return _style_msg;
            }
        }

        private string mCurInput;
        private bool btn_Confirm_clicked = false;

        private void OnEnable()
        {
            mCurInput = ScriptableSingleton<PromptParam>.instance.defaultContent;
        }

        private void OnGUI()
        {
            if (!ScriptableSingleton<PromptParam>.instance.messageContent.IsNullOrEmpty())
            {
                GUILayout.Label(ScriptableSingleton<PromptParam>.instance.messageContent, style_msg);
            }
            mCurInput = EditorGUILayout.TextField(mCurInput);

            //btns
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ScriptableSingleton<PromptParam>.instance.confirm_text))
            {
                btn_Confirm_clicked = true;
                this.Close();
            }
            if (!ScriptableSingleton<PromptParam>.instance.cancel_text.IsNullOrEmpty())
            {
                if (GUILayout.Button(ScriptableSingleton<PromptParam>.instance.cancel_text))
                {
                    btn_Confirm_clicked = false; //这句话其实不需要，作用是让强迫症舒服一点
                    this.Close();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            ScriptableSingleton<PromptParam>.instance.callback?.Invoke(btn_Confirm_clicked, mCurInput);
            ScriptableSingleton<PromptParam>.instance.ClearData();
        }

    }

    internal class PromptParam : ScriptableSingleton<PromptParam>
    {
#pragma warning disable IDE1006 // 命名样式

        public Action<bool, string> callback { get; set; }
        public string defaultContent { get; set; }
        public string messageContent { get; set; }
        public GUIContent title { get; set; }

        public string confirm_text { get; set; }
        public string cancel_text { get; set; }

#pragma warning restore IDE1006 // 命名样式

        public void ClearData()
        {
            callback = null;
            defaultContent = null;
            messageContent = null;
            title = null;
            confirm_text = "Confirm";
            cancel_text = null;
        }
    }
}
