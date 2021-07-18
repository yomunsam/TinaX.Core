using System;
using System.Reflection;
using CatLib.Container;
using TinaX.Packages.io.nekonya.tinax.core.Runtime.Scripts.Container.Internal;
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
            if(this.CanMake<IGetServiceName>())
            {
                return this.Make<IGetServiceName>().GetServiceName(propertyInfo.PropertyType);
            }
            return base.GetPropertyNeedsService(propertyInfo);
        }




    }
}
