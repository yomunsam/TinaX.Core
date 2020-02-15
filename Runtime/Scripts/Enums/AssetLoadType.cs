using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX
{
    public enum AssetLoadType
    {
        /// <summary>
        /// UnityEngine.Resources
        /// </summary>
        Resources,
        /// <summary>
        /// TinaX VFS
        /// </summary>
        VFS,
        /// <summary>
        /// System I/O, eg: System.IO.File.LoadFile(xxxx)
        /// </summary>
        SystemIO,
    }
}
