using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using TinaX;
using TinaX.IO;
using TinaXEditor.Const;
using TinaXEditor.Internal;

namespace TinaXEditor
{
    [InitializeOnLoad]
    public static class XCoreEditor
    {
        static XCoreEditor()
        {
            //检查 TinaX Profile
            XDirectory.CreateIfNotExists(XEditorConst.EditorProjectSettingRootFolder);

            RefreshXProfile();
        }

        private static XProfileModel mProfiles_obj;
        private static List<string> mProfiles;


        public static string[] GetXProfiles()
        {
            if (mProfiles == null) mProfiles = new List<string>();
            if (mProfiles.Count == 0) mProfiles.Add(XEditorConst.DefaultProfileName);
            return mProfiles.ToArray();
        }

        public static void RefreshXProfile()
        {
            mProfiles_obj = null;
            bool create = false;
            List<string> temp_list = new List<string>();
            if (File.Exists(XEditorConst.EditorXProfilePath))
            {
                //load
                try
                {
                    var json_str = File.ReadAllText(XEditorConst.EditorXProfilePath, encoding: Encoding.UTF8);
                    mProfiles_obj = JsonUtility.FromJson<XProfileModel>(json_str);
                    temp_list.AddRange(mProfiles_obj.ProfileNames);
                }
                catch
                {
                    create = true;
                }
            }
            

            if(create)
            {
                mProfiles_obj = new XProfileModel();
                temp_list.AddRange(mProfiles_obj.ProfileNames);
            }

            if (mProfiles == null)
                mProfiles = new List<string>();
            else
                mProfiles.Clear();

            foreach(var item in temp_list)
            {
                if(!mProfiles.Any(p => p.ToLower() == item.ToLower()))
                {
                    mProfiles.Add(item);
                }
            }


        }

        public static void SaveXProfiles()
        {
            if(mProfiles == null || mProfiles.Count == 0)
            {
                mProfiles = new List<string>();
                mProfiles.Add(XEditorConst.DefaultProfileName);
            }

            if (mProfiles_obj == null) mProfiles_obj = new XProfileModel();
            mProfiles_obj.ProfileNames = mProfiles.ToArray();

            if (File.Exists(XEditorConst.EditorXProfilePath))
                File.Delete(XEditorConst.EditorXProfilePath);

            var json_str = JsonUtility.ToJson(mProfiles_obj);
            File.WriteAllText(XEditorConst.EditorXProfilePath, json_str, Encoding.UTF8);
        }

        public static bool AddXProfile(string profileName)
        {
            if (mProfiles == null) mProfiles = new List<string>();
            if (mProfiles.Count == 0) mProfiles.Add(XEditorConst.DefaultProfileName);

            if (profileName.IsNullOrEmpty() || profileName.IsNullOrWhiteSpace())
            {
                Debug.LogError($"[TinaX] Add profile name error : profile name cannot be empty or whitespaces");
                return false;
            }

            if (mProfiles.Any(p => p.ToLower() == profileName.ToLower()))
            {
                Debug.LogError($"[TinaX] Add profile name error : cannot add the same profile name \"{profileName}\"");
                return false;
            }

            mProfiles.Add(profileName);
            return true;
        }

        public static void RemoveXProfile(string profileName)
        {
            if (mProfiles == null) mProfiles = new List<string>();
            if (mProfiles.Count == 0) mProfiles.Add(XEditorConst.DefaultProfileName);

            if (mProfiles.Count < 1) return; //至少保留一个

            var result = mProfiles.Where(p => p.ToLower() == profileName.ToLower());
            if (result.Count() <= 0) return; 

            

            mProfiles.Remove(result.First());

        }

        public static string GetCurrentActiveXProfileName()
        {
            return mProfiles_obj.CurrentProfileName;
        }

        public static bool SetActiveXProfile(string profileName)
        {
            if (mProfiles == null) mProfiles = new List<string>();
            if (mProfiles.Count == 0) mProfiles.Add(XEditorConst.DefaultProfileName);

            if (profileName.IsNullOrEmpty() || profileName.IsNullOrWhiteSpace())
            {
                Debug.LogError("[TinaX] set active profile name error : profile name cannot be empty or whitespaces");
                return false;
            }

            if (!mProfiles.Any(p => p.ToLower() == profileName.ToLower()))
            {
                Debug.LogError($"[TinaX] set active profile name error : profile name \"{profileName}\" not found ");
                return false;
            }

            mProfiles_obj.CurrentProfileName = profileName;
            return true;
        }


    }
}
