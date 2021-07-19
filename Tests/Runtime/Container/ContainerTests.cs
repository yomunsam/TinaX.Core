using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TinaX;
using UnityEngine;
using UnityEngine.TestTools;

namespace TinaX.Core.Tests.Container
{
    public class ContainerTests
    {
        private IXCore m_Core;

        [SetUp]
        public void Setup()
        {
            m_Core = XCore.CreateDefault();
            

            
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ContainerTestsSimplePasses()
        {
            m_Core.Services.Bind<IMeowA, MeowA>();
            m_Core.Services.Bind<MeowB>();
            var b1 = m_Core.Services.Get<MeowB>();
            Assert.AreEqual(b1.Trigger(), 1);
            Assert.AreEqual(b1.Trigger(), 2);
            Assert.AreEqual(b1.Trigger(), 3);

            var b2 = m_Core.Services.Get<MeowB>();
            Assert.AreEqual(b2.Trigger(), 1);
            Assert.AreEqual(b2.Trigger(), 2);

            m_Core.Services.Unbind<MeowB>();
            m_Core.Services.Singleton<MeowB>();
            Assert.AreEqual(m_Core.Services.Get<MeowB>().Trigger(), 1);
            Assert.AreEqual(m_Core.Services.Get<MeowB>().Trigger(), 2);
            Assert.AreEqual(m_Core.Services.Get<MeowB>().Trigger(), 3);
        }
    }


    public interface IMeowA
    {
        int Trigger();
    }

    public class MeowA : IMeowA
    {
        public int Counter = 0;

        public int Trigger()
        {
            return ++Counter;
        }
    }

    public class MeowB
    {
        private readonly IMeowA a;

        public MeowB(IMeowA a)
        {
            this.a = a;
        }

        public int Trigger() => a.Trigger();
    }

}



