using System.Collections.Generic;

namespace TinaX
{
    public interface ICommandLineArgs
    {
        IDictionary<string, string> GetAllKeyValueArgs();
        IList<string> GetAllSingleArgs();

        bool GetBool(string key);
        string GetValue(string key, string defaultValue = null);
        bool IsExistKey(string key);
        bool IsExistSingle(string name);
        bool TryGetValue(string key, out string value);
    }
}
