using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace TinaX.Options.Internal
{
    public class OptionsManager : IOptionsManager
    {
        private Hashtable m_OptionsTable = new Hashtable();
        /*
         * m_OptionsTable = {
         *      ["typeName"] = 
         *      {
         *          ["name"] = options
         *      }
         * }
         * 
         * Hashtable[总表, key:typeName] --> Hashtable[key: name]
         */

        public void Set(string typeName, string name ,Options options)
        {
            Hashtable subTable;
            if (m_OptionsTable.ContainsKey(typeName))
            {
                subTable = (Hashtable)m_OptionsTable[typeName];
            }
            else
            {
                subTable = new Hashtable();
            }

            if (subTable.ContainsKey(name))
            {
                subTable[name] = options;
            }
            else
            {
                subTable.Add(name, options);
            }
        }

        public bool TryGet(string typeName, string name, out Options options)
        {
            if (m_OptionsTable.ContainsKey(typeName))
            {
                var subTable = (Hashtable)m_OptionsTable[typeName];
                if(subTable.ContainsKey(name))
                {
                    options = (Options)subTable[name];
                    return true;
                }    
            }

            options = default;
            return false;
        }

        public IOptions GetOptions(Type type) => GetOptions(type.FullName, Options.DefaultName);
        public IOptions GetOptions(Type type, string name) => GetOptions(type.FullName, name);

        public IOptions GetOptions(string typeName) => GetOptions(typeName, Options.DefaultName);

        public IOptions GetOptions(string typeName , string name)
        {
            if(TryGet(typeName, name, out var opt))
            {
                return opt;
            }
            return null;
        }

    }
}
