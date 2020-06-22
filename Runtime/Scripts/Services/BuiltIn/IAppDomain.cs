using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Services
{
    public interface IAppDomain : IBuiltInService
    {
        object CreateInstance(Type type, params object[] args);
        bool TryGetServiceName(Type type, out string name);
    }
}
