using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TinaX
{
    public static class UObjectExtend
    {
        public static bool IsNull(this Object target)
        {
            return target == null;
        }
    }
}
