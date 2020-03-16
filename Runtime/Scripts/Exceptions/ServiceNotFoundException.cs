using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX
{
    public class ServiceNotFoundException : XException
    {
        public Type ServiceType { get; private set; }
        public ServiceNotFoundException(Type service) : base(msg: "Service Not Found: " + service.Name)
        {
            this.ServiceType = service;
        }

        public ServiceNotFoundException(string msg, Type service): base(msg)
        {
            this.ServiceType = service;
        }

    }
}
