using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Services
{
    public interface IEventTicket
    {

    }

    public struct EventTicket : IEventTicket
    {
        public Action<object> handler;
        public string group;
        public string name;
    }
}
