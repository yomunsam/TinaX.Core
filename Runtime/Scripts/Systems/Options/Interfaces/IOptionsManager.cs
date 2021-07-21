using System;

namespace TinaX.Options
{
    public interface IOptionsManager
    {
        IOptions GetOptions(string typeName, string name);
        IOptions GetOptions(Type type);
        IOptions GetOptions(string typeName);
    }
}
