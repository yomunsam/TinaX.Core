using System;
using System.Reflection;
using CatLib.Container;
using TinaX.Container;
using CatApplication = CatLib.Application;

namespace TinaX.Catlib
{
    public class XCatApplication : CatApplication
    {
        public XCatApplication()
        {

        }

        protected override string GetPropertyNeedsService(PropertyInfo propertyInfo)
        {
            return GetServiceName(propertyInfo.PropertyType);
        }


        public virtual string GetServiceName(Type type)
        {
            if(this.CanMake<IGetServiceName>())
            {
                if (this.Make<IGetServiceName>().TryGetServiceName(type, out string name))
                    return name;
            }
            return Type2Service(type);
        }

    }
}
