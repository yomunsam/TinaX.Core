using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        public bool Nullable = false;
        public InjectAttribute() { }
        public InjectAttribute(bool nullable = false) { this.Nullable = nullable; }
    }
}
