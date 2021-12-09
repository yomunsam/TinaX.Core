using System;
using System.Reflection;
using CatLib.Container;
using UnityEngine;
using CatApplication = CatLib.Application;

namespace TinaX.Catlib
{
    public class XCatApplication : CatApplication
    {
        private readonly XCore m_Core;
        private readonly ServiceContainer m_ServiceContainer;

        public XCatApplication(XCore core, ServiceContainer serviceContainer)
        {
            this.m_Core = core;
            this.m_ServiceContainer = serviceContainer;
        }

        
        
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="makeServiceType"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        protected override object CreateInstance(Type makeServiceType, object[] userParams)
        {
            if(m_Core.Activator.TryCreateInstance(makeServiceType, out var instance, userParams))
            {
                return instance;
            }
            return base.CreateInstance(makeServiceType, userParams);
        }

        protected override string GetPropertyNeedsService(PropertyInfo propertyInfo)
        {
            //Debug.Log($"GetPropertyNeedsService:{propertyInfo.Name} - {propertyInfo.GetType().FullName}");

            return GetServiceName(propertyInfo.PropertyType);
        }

        /// <summary>
        /// 获取参数需求的服务
        /// </summary>
        /// <param name="baseParam"></param>
        /// <returns></returns>
        protected override string GetParamNeedsService(ParameterInfo baseParam)
        {
            //Debug.Log($"GetParamNeedsService:{baseParam.Name} - {baseParam.GetType().FullName}");
            return base.GetParamNeedsService(baseParam);
        }

        protected override void AttributeInject(Bindable makeServiceBindData, object makeServiceInstance)
        {
            if (makeServiceInstance == null)
                return;
            //Debug.Log($"[{nameof(XCatApplication)}]属性注入被调用:{makeServiceBindData.Service}  -- instance:{makeServiceInstance.GetType().FullName}");
            if (m_ServiceContainer.TryServiceAttributeInject(ref makeServiceBindData, ref makeServiceInstance))
            {
                return;
            }
            base.AttributeInject(makeServiceBindData, makeServiceInstance);
        }

        protected override object ResloveAttrClass(Bindable makeServiceBindData, string service, PropertyInfo baseParam)
        {
            //Debug.Log($"[{nameof(XCatApplication)}]ResloveAttrClass被调用:{makeServiceBindData.Service}");
            return base.ResloveAttrClass(makeServiceBindData, service, baseParam);
        }



        public virtual string GetServiceName(Type type)
            => Type2Service(type);

    }
}
