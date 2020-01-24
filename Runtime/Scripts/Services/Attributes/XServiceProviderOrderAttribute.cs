using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinaX.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XServiceProviderOrderAttribute : Attribute
    {
        public int Order = 100;

        public XServiceProviderOrderAttribute(int order = 100)
        {
            Order = order;
        }
    }
}


