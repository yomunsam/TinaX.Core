using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Options
{
    public interface IOptionsManager
    {
        IOptions GetOptions(string typeName, string name);
        IOptions GetOptions(Type type);
        IOptions GetOptions(string typeName);
    }
}
