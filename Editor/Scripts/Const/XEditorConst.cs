using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace TinaXEditor.Const
{
    public static class XEditorConst
    {
        /// <summary>
        /// Path: /ProjectSettings/TinaX
        /// </summary>
        public static string EditorProjectSettingRootFolder
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "ProjectSettings", "TinaX");
            }
        }

        public static string EditorXProfilePath => Path.Combine(EditorProjectSettingRootFolder, "xProfiles.json");


        public static string ProjectSettingRootName = "TinaX Framework";

        public static string ProjectSetting_CorePath = ProjectSettingRootName + "/Core";

        internal static string DefaultProfileName = "Default";

        /// <summary>
        /// path: /TinaX
        /// </summary>
        public static string TinaXProjectRootFolderPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "TinaX");
            }
        }

        /// <summary>
        /// path: /Library/TinaX
        /// </summary>
        public static string TinaXProjectLibraryRootFolder
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Library", "TinaX");
            }
        }

    }
}
