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
        #region 用于序列化存储
        public XProfileItem[] Items = {  };

        public string CurrentProfileName = string.Empty;

        #endregion

        #region functions

        private List<XProfileItem> _list_items;
        private List<XProfileItem> list_items
        {
            get
            {
                if(_list_items == null)
                {
                    if (Items == null) Items = new XProfileItem[0];
                    _list_items = new List<XProfileItem>(Items);
                }
                return _list_items;
            }
        }

        public string[] GetXProfileNames()
        {
            string[] tmp = new string[list_items.Count];
            for(var i = 0;i < list_items.Count; i++)
            {
                tmp[i] = list_items[i].ProfileName;
            }
            return tmp;
        }

        public void AddXProfile(string name)
        {
            if (IsXProfileExists(name))
                return;
            list_items.Add(XProfileItem.GetDefault(name));
        }

        public bool IsXProfileExists(string name)
        {
            return list_items.Any(i => i.ProfileName.ToLower() == name.ToLower());
        }

        public void RemoveXProfile(string name)
        {
            for(var i = list_items.Count - 1; i >= 0; i--)
            {
                if(list_items[i].ProfileName.ToLower() == name.ToLower())
                {
                    list_items.RemoveAt(i);
                }
            }

            if(list_items.Count == 0)
            {
                list_items.Add(XProfileItem.GetDefault());
            }
        }

        public void ReadySave()
        {
            Items = list_items.ToArray();
        }

        public int GetCount()
        {
            return list_items.Count;
        }

        public void SetDevelopMode(string name ,bool isDevelopMode)
        {
            for(var i = 0; i < list_items.Count; i++)
            {
                if(list_items[i].ProfileName.ToLower() == name.ToLower())
                {
                    var t = list_items[i];
                    t.DevelopMode = isDevelopMode;
                    list_items[i] = t;
                    break;
                }
            }
        }

        public bool IsDevelopMode(string name)
        {
            foreach(var item in list_items)
            {
                if (item.ProfileName.ToLower() == name.ToLower())
                    return item.DevelopMode;
            }
            return false;
        }

        /// <summary>
        /// 检查是否含有默认的profile
        /// </summary>
        public void CheckDefaultProfile()
        {
            if(list_items.Count == 0)
            {
                list_items.Add(XProfileItem.GetDefault());
            }
        }

        #endregion

        public static XProfileModel GetDefault()
        {
            var temp = new XProfileModel();
            //包含一个Default Item
            var default_item = XProfileItem.GetDefault();
            temp.Items = new XProfileItem[] { default_item };
            temp.CurrentProfileName = default_item.ProfileName;
            return temp;
        }

    }
}
