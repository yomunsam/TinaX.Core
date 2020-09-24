using System;
using System.Linq;
using TinaX;
using TinaX.Internal;
using TinaX.Const;
using TinaXEditor.Const;
using UnityEngine;
using UnityEditor;
using TinaXEditor.Utils;
using UnityEditorInternal;

namespace TinaXEditor.ProjectSetting
{
    static class CoreSetting
    {
        private static GUIStyle _style_txt_h2;
        private static GUIStyle style_txt_h2
        {
            get
            {
                if(_style_txt_h2 == null)
                {
                    _style_txt_h2 = new GUIStyle(EditorStyles.label);
                    _style_txt_h2.normal.textColor = XEditorColorDefine.Color_Normal;
                    _style_txt_h2.fontSize = 18;
                }
                return _style_txt_h2;
            }
        }
        
        private static GUIStyle _style_txt_profile_item;
        private static GUIStyle style_txt_profile_item
        {
            get
            {
                if(_style_txt_profile_item == null)
                {
                    _style_txt_profile_item = new GUIStyle(EditorStyles.label);
                    _style_txt_profile_item.normal.textColor = XEditorColorDefine.Color_Normal_Pure;
                    _style_txt_profile_item.fontSize = 13;
                    _style_txt_profile_item.fontStyle = FontStyle.BoldAndItalic;
                    _style_txt_profile_item.padding.left = 20;
                }
                return _style_txt_profile_item;
            }
        }

        private static GUIStyle _style_txt_active;
        private static GUIStyle style_txt_active
        {
            get
            {
                if (_style_txt_active == null)
                {
                    _style_txt_active = new GUIStyle(EditorStyles.label);
                    _style_txt_active.normal.textColor = XEditorColorDefine.Color_Safe;
                }
                return _style_txt_active;
            }
        }

        private static GUIStyle _style_txt_blue;
        private static GUIStyle style_txt_blue
        {
            get
            {
                if (_style_txt_blue == null)
                {
                    _style_txt_blue = new GUIStyle(EditorStyles.label);
                    _style_txt_blue.normal.textColor = XEditorColorDefine.Color_Emphasize;
                }
                return _style_txt_blue;
            }
        }

        private static GUIStyle _style_txt_warning;
        private static GUIStyle style_txt_warning
        {
            get
            {
                if (_style_txt_warning == null)
                {
                    _style_txt_warning = new GUIStyle(EditorStyles.label);
                    _style_txt_warning.normal.textColor = XEditorColorDefine.Color_Warning;
                }
                return _style_txt_warning;
            }
        }

        private static string[] xprofiles;
        private static int select_xprofile;
        private static bool b_flodout_show_xprofiles_detail;
        private static string input_new_profile_name;

        private static bool? cache_isDevelopMode;
        private static string cached_developMode_profile; //上面这个变量是哪个profile的信息

        /// <summary>
        /// Debug 调试命令行设置
        /// </summary>
        private static string m_DebugCommandLineArgs;
        private static bool m_loadedDebugCommandLineArgs; //是否加载过一次debug命令行参数

        [SettingsProvider]
        public static SettingsProvider CoreSettingPage()
        {
            return new SettingsProvider(XEditorConst.ProjectSetting_CorePath, SettingsScope.Project, new string[] { "Nekonya", "TinaX", "Core", "TinaX.Core" })
            {
                label = "X Core",
                activateHandler = (searchContent,uielementRoot) =>
                {
                    XCoreEditor.RefreshXProfile();
                    m_DebugCommandLineArgs = XCoreEditor.GetDebugCommandLineArgs();
                    m_loadedDebugCommandLineArgs = true;
                },
                guiHandler = (searchContent) =>
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    #region TinaX Profile
                    GUILayout.Label("TinaX Profile", style_txt_h2);
                    GUILayout.Space(2);

                    GUILayout.BeginHorizontal();
                    if (xprofiles == null)
                    {
                        refreshXprofilesCacheData();
                    }

                    GUILayout.Label(i18n_label_cur_active_profile, GUILayout.MaxWidth(140));
                    select_xprofile = EditorGUILayout.Popup(select_xprofile, xprofiles, GUILayout.MaxWidth(290));

                    if (xprofiles[select_xprofile] != XCoreEditor.GetCurrentActiveXProfileName())
                    {
                        if (GUILayout.Button(i18n_btn_switch_active_profile, GUILayout.MaxWidth(80)))
                        {
                            if (!XCoreEditor.SetActiveXProfile(xprofiles[select_xprofile]))
                            {
                                EditorUtility.DisplayDialog("Error", "Set active profile failed, please check the details in the console", "Okey");
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    #region 是否为开发模式
                    //数据
                    if (cached_developMode_profile.IsNullOrEmpty() || cache_isDevelopMode == null || cached_developMode_profile != xprofiles[select_xprofile])
                    {
                        cache_isDevelopMode = XCoreEditor.IsXProfileDevelopMode(xprofiles[select_xprofile]);
                        cached_developMode_profile = xprofiles[select_xprofile];
                    }
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(IsChinese ? "开发模式：" : "Develop Mode : ", GUILayout.Width(80));
                    GUILayout.Label(cache_isDevelopMode.Value ? (IsChinese ? "[已启用]" : "[Enabled]") : (IsChinese ? "[未启用]" : "[Disabled]"), (cache_isDevelopMode.Value ? style_txt_warning : style_txt_blue), GUILayout.Width(50));
                    if (GUILayout.Button(cache_isDevelopMode.Value ? (IsChinese ? "关闭" : "Disable") : (IsChinese ? "启用" : "Enable"),GUILayout.Width(50)))
                    {
                        XCoreEditor.SetXProfileDevelopMode(xprofiles[select_xprofile], !cache_isDevelopMode.Value);
                        cache_isDevelopMode = null;
                        cached_developMode_profile = string.Empty;
                        XCoreEditor.SaveXProfiles();
                    }
                    GUILayout.EndHorizontal();

                    #endregion

                    b_flodout_show_xprofiles_detail = EditorGUILayout.Foldout(b_flodout_show_xprofiles_detail, i18n_profiles_detail);
                    if (b_flodout_show_xprofiles_detail)
                    {
                        //列表
                        if(xprofiles != null && xprofiles.Length > 0)
                        {
                            int length = xprofiles.Length;
                            foreach(var name in xprofiles)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(name, style_txt_profile_item,GUILayout.MaxWidth(260));
                                if(length > 1)
                                {
                                    if(XCoreEditor.GetCurrentActiveXProfileName() == name)
                                    {
                                        GUILayout.Button("[Active]", style_txt_active, GUILayout.Width(50));
                                    }
                                    else
                                    {
                                        if (GUILayout.Button(i18n_profiles_detail_remove, GUILayout.Width(50)))
                                        {
                                            
                                            XCoreEditor.RemoveXProfile(name);
                                            refreshXprofilesCacheData();

                                            break;
                                        }
                                    }
                                    
                                }
                                GUILayout.EndHorizontal();
                            }
                        }

                        //添加
                        EditorGUILayout.Space();
                        //EditorGUIUtil.HorizontalLine();
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(i18n_profiles_detail_label_new,GUILayout.Width(126) );
                        input_new_profile_name = EditorGUILayout.TextArea(input_new_profile_name, GUILayout.Width(220));
                        if (GUILayout.Button(i18n_profiles_detail_create, GUILayout.Width(50)))
                        {
                            if (input_new_profile_name.IsNullOrEmpty() || input_new_profile_name.IsNullOrWhiteSpace()) return;
                            //查重判断
                            if(xprofiles.Any(p => p.ToLower() == input_new_profile_name.ToLower()))
                            {
                                EditorUtility.DisplayDialog("Error", string.Format(i18n_profiles_detail_create_same_name, input_new_profile_name), "Okey");
                                return;
                            }
                            if (!XCoreEditor.AddXProfile(input_new_profile_name))
                            {
                                EditorUtility.DisplayDialog("Error", "Create profile failed, please check the details in the console", "Okey");
                            }
                            else
                            {
                                input_new_profile_name = string.Empty;
                                XCoreEditor.SaveXProfiles();
                                xprofiles = XCoreEditor.GetXProfileNames();
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(8);
                        EditorGUIUtil.HorizontalLine();
                    }
                    #endregion

                    GUILayout.Space(20);
                    EditorGUIUtil.HorizontalLine();
                    EditorGUILayout.Space();
                    #region Debug Command Line
                    EditorGUILayout.LabelField(i18n_debug_command_line_args);
                    m_DebugCommandLineArgs = EditorGUILayout.TextArea(m_DebugCommandLineArgs);

                    #endregion
                },
                deactivateHandler = () =>
                {
                    XCoreEditor.SaveXProfiles();
                    if (m_loadedDebugCommandLineArgs)
                        XCoreEditor.SaveDebugCommandLineArgs(m_DebugCommandLineArgs);
                }
            };
        }

        static void refreshXprofilesCacheData()
        {
            xprofiles = XCoreEditor.GetXProfileNames();
            //get cur index 
            int cur_index = 0;
            string cur_name = XCoreEditor.GetCurrentActiveXProfileName();
            for (var i = 0; i < xprofiles.Length; i++)
            {
                if (xprofiles[i] == cur_name)
                {
                    cur_index = i;
                    break;
                }
            }
            select_xprofile = cur_index; 
        }


        #region I18N
        private static bool? _isChinese;
        private static bool IsChinese
        {
            get
            {
                if (_isChinese == null)
                {
                    _isChinese = (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified);
                }
                return _isChinese.Value;
            }
        }

        private static string i18n_label_cur_active_profile
        {
            get
            {
                if (IsChinese)
                    return "当前使用Profile:";
                return "Current active profile:";
            }
        }

        private static string i18n_btn_switch_active_profile
        {
            get
            {
                if (IsChinese)
                    return "切换Profile";
                return "Apply";
            }
        }

        private static string i18n_profiles_detail
        {
            get
            {
                if (IsChinese)
                    return "Profile 细节";
                return "Profile Details";
            }
        }

        private static string i18n_profiles_detail_remove
        {
            get
            {
                if (IsChinese)
                    return "删除";
                return "Remove";
            }
        }

        private static string i18n_profiles_detail_create
        {
            get
            {
                if (IsChinese)
                    return "创建";
                return "Create";
            }
        }

        private static string i18n_profiles_detail_create_same_name
        {
            get
            {
                if (IsChinese)
                    return "当前已经有名为“{0}”的profile，不可以创建同名profile.";
                return "A profile named '{0}' already exists. You cannot create a profile with the same name";
            }
        }

        private static string i18n_profiles_detail_label_new
        {
            get
            {
                if (IsChinese)
                    return "创建新Profile: ";
                return "Create a new profile: ";
            }
        }


        private static string i18n_debug_command_line_args
        {
            get
            {
                if (IsChinese)
                    return "调试启动命令行：";
                return "Debug Command Line Args: ";
            }
        }
        #endregion

    }


}
