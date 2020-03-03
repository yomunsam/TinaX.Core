using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX
{

    /// <summary>
    /// A universal attribute about priority order in TinaX | TinaX中一个通用的表示优先级次序的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get;  private set; }

        public PriorityAttribute(int priority)
        {
            this.Priority = priority;
        }

        public PriorityAttribute()
        {
            this.Priority = 100;
        }
    }
}
