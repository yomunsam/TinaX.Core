using System;
using TinaX;

namespace TinaXEditor.Internal
{
    [Serializable]
    public struct XProfileItem
    {
        public string ProfileName;
        public bool DevelopMode;

        public static XProfileItem GetDefault(string name = null)
        {
            var item = new XProfileItem();
            item.DevelopMode = false;
            if (!name.IsNullOrEmpty() && !name.IsNullOrWhiteSpace())
                item.ProfileName = name;
            else
                item.ProfileName = TinaXEditor.Const.XEditorConst.DefaultProfileName;

            return item;
        }

        

    }
}
