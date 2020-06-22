using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CatLib;
using UnityEngine;
using CatApplication = CatLib.Application;

namespace TinaX.Internal
{
    public class XCatApplication : CatApplication
    {
        private XCore m_Core;
        public XCatApplication(XCore core)
        {
            m_Core = core;
        }

        protected override object CreateInstance(Type makeServiceType, object[] userParams)
        {
            return m_Core.CreateInstance(makeServiceType, userParams);
        }

        protected override string GetParamNeedsService(ParameterInfo baseParam)
        {
            return m_Core.GetServiceName(baseParam.ParameterType);
        }

        protected override string GetPropertyNeedsService(PropertyInfo propertyInfo)
        {
            return m_Core.GetServiceName(propertyInfo.PropertyType);
        }
    }
}
