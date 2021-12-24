using System;
using System.Reflection;
using CatLib.Container;
using CatApplication = CatLib.Application;

namespace TinaX.Container
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


        protected override object ResloveAttrClass(Bindable makeServiceBindData, string service, PropertyInfo baseParam)
        {
            //Debug.Log($"[{nameof(XCatApplication)}]ResloveAttrClass被调用:{makeServiceBindData.Service}");
            return base.ResloveAttrClass(makeServiceBindData, service, baseParam);
        }



        public virtual string GetServiceName(Type type)
            => Type2Service(type);

        /// <summary>
        /// 帮助容器获取类型
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        protected override Type GetType(ref object sourceObject)
        {
            if (m_ServiceContainer.TryGetType(ref sourceObject, out var type))
                return type;
            else
                return base.GetType(ref sourceObject);
        }

        /// <summary>
        /// 检查给定的属性是否可以被依赖注入
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool CanPropertyInject(PropertyInfo property)
        {
            var b = m_ServiceContainer.CanInjected(ref property);
            if(b.HasValue)
                return b.Value;

            return property.CanWrite && property.IsDefined(typeof(TinaX.InjectAttribute), true);
        }

        protected override bool CanProjectInjectSkip(PropertyInfo property)
        {
            var b = m_ServiceContainer.CanInjectedSkip(ref property);
            if (b.HasValue)
                return b.Value;

            var attribute = property.GetCustomAttribute<TinaX.InjectAttribute>();
            if (attribute != null)
                return attribute.Nullable;
            else
                return true;
        }
    }
}
