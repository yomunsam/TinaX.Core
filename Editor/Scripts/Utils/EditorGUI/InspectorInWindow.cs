using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX;
using UnityEngine;
using UnityEditor;

namespace TinaXEditor.Utils
{
    public class InspectorInWindow : EditorWindow
    {
        static UnityEngine.Object _target_obj;
        static InspectorInWindow wnd;
        public static void ShowInspector(UnityEngine.Object _target)
        {
            if (wnd != null)
                wnd.Close();
            wnd = null;
            _target_obj = null;

            _target_obj = _target;
            OpenUI();
        }

        private static void OpenUI()
        {
            if (_target_obj == null) return;
            if(wnd == null)
            {
                wnd = GetWindow<InspectorInWindow>();
                wnd.titleContent = new GUIContent($"Inspector: [{_target_obj.name}]-{_target_obj.GetType().Name}");
            }
            else
            {
                wnd.Show();
                wnd.Focus();
            }
        }


        Editor _target_editor;

        private void OnGUI()
        {
            if(_target_obj == null)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Target Object lost, Please reopen this window.");
                GUILayout.FlexibleSpace();
            }
            else
            {
                GUILayout.Label($"[{_target_obj.name}]-{_target_obj.GetType().Name}");
                if (_target_editor == null)
                    _target_editor = Editor.CreateEditor(_target_obj);
                if (_target_editor != null)
                    _target_editor.DrawDefaultInspector();
            }
        }

        private void OnDestroy()
        {
            wnd = null;
            _target_obj = null;
        }

    }
}
