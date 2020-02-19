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

        private static string[] xprofiles;
        private static int select_xprofile;
        private static bool b_flodout_show_xprofiles_detail;
        private static string input_new_profile_name;

        [SettingsProvider]
        public static SettingsProvider CoreSettingPage()
        {
            return new SettingsProvider(XEditorConst.ProjectSetting_CorePath, SettingsScope.Project, new string[] { "Nekonya", "TinaX", "Core", "TinaX.Core" })
            {
                label = "X Core",
                activateHandler = (searchContent,uielementRoot) =>
                {
                    XCoreEditor.RefreshXProfile();
                },
                guiHandler = (searchContext) =>
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    #region TinaX Profile
                    GUILayout.Label("TinaX Profile",style_txt_h2);
                    GUILayout.Space(2);

                    GUILayout.BeginHorizontal();
                    if (xprofiles == null)
                    {
                        xprofiles = XCoreEditor.GetXProfiles();
                        //get cur index 
                        int cur_index = 0;
                        string cur_name = XCoreEditor.GetCurrentActiveXProfileName();
                        for(var i = 0; i < xprofiles.Length; i++)
                        {
                            if (xprofiles[i] == cur_name)
                            {
                                cur_index = i;
                                break;
                            }
                        }
                        select_xprofile = cur_index;
                    }
                    GUILayout.Label(i18n_label_cur_active_profile,GUILayout.MaxWidth(140));
                    select_xprofile = EditorGUILayout.Popup(select_xprofile, xprofiles,GUILayout.MaxWidth(290));
                    if(xprofiles[select_xprofile] != XCoreEditor.GetCurrentActiveXProfileName())
                    {
                        if (GUILayout.Button(i18n_btn_switch_active_profile,GUILayout.MaxWidth(80)))
                        {
                            if (!XCoreEditor.SetActiveXProfile(xprofiles[select_xprofile]))
                            {
                                EditorUtility.DisplayDialog("Error", "Set active profile failed, please check the details in the console","Okey");
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

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
                                            xprofiles = XCoreEditor.GetXProfiles();
                                            break;
                                        }
                                    }
                                    
                                }
                                GUILayout.EndHorizontal();
                            }
                        }

                        //添加
                        EditorGUIUtil.HorizontalLine();
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
                                xprofiles = XCoreEditor.GetXProfiles();
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUIUtil.HorizontalLine();
                    }
                    #endregion
                },
                deactivateHandler = () =>
                {
                    XCoreEditor.SaveXProfiles();
                }
            };
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

        #endregion

    }


}
