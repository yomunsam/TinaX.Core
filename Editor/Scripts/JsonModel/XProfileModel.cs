using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TinaXEditor.Internal
{
    [Serializable]
    public class XProfileModel
    {
        public string[] ProfileNames = { TinaXEditor.Const.XEditorConst.DefaultProfileName };

        public string CurrentProfileName = TinaXEditor.Const.XEditorConst.DefaultProfileName;
    }
}
