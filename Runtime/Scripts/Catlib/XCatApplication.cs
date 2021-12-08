using System;
using System.Reflection;
using CatLib.Container;
using TinaX.Container;
using TinaX.Core.Container;
using TinaX.Services;
using UnityEngine;
using CatApplication = CatLib.Application;

namespace TinaX.Catlib
{
    public class XCatApplication : CatApplication
    {
        public XCatApplication()
        {
            
        }

        public IInstanceCreator InstanceCreator { get; set; } //实例创建器


        protected override string GetPropertyNeedsService(PropertyInfo propertyInfo)
        {
            return GetServiceName(propertyInfo.PropertyType);
        }
        
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="makeServiceType"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        protected override object CreateInstance(Type makeServiceType, object[] userParams)
        {
            if(InstanceCreator != null)
            {
                if (InstanceCreator.TryCreateInstance(makeServiceType, out var obj, userParams))
                    return obj;
            }
            return base.CreateInstance(makeServiceType, userParams);
        }

        protected override void AttributeInject(Bindable makeServiceBindData, object makeServiceInstance)
        {
            Debug.Log($"[{nameof(XCatApplication)}]属性注入被调用");
            base.AttributeInject(makeServiceBindData, makeServiceInstance);
        }

        public virtual string GetServiceName(Type type)
            => Type2Service(type);

    }
}
