using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TinaX.Internal;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArgsManagerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ArgsManagerTestSimplePasses()
        {
            var mgr = new ArgsManager();
            var args = new string[] { };
        }

        //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        //// `yield return null;` to skip a frame.
        //[UnityTest]
        //public IEnumerator ArgsManagerTestWithEnumeratorPasses()
        //{
        //    // Use the Assert class to test conditions.
        //    // Use yield to skip a frame.
        //    yield return null;
        //}
    }
}
