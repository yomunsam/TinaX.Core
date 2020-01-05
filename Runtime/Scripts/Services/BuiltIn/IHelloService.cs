using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Services
{
    public interface IHello : IBuiltInService
    {
        string SayHello(string msg);
    }
}
