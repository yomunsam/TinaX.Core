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
        public static string EditorProjectSettingRootFolder
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "ProjectSettings", "TinaX");
            }
        }
    }
}
