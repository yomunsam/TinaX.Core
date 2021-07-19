using System;

namespace TinaX.Modules
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleProviderOrderAttribute : Attribute
    {
        public const int DefaultOrder = 100;

        public int Order { get; private set; } = DefaultOrder;

        public ModuleProviderOrderAttribute(int order = 100)
        {
            Order = order;
        }
    }
}
