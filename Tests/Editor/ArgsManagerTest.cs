using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TinaX;
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
            string sArg = "--name = Eriri --full-name = \"Sawamura Spencer Eriri\" --enable --CV= \"Oonishi Saori\"";
            string[] args = ArgsUtil.ParseArgsText(sArg).ToArray();
            var mgr = new ArgsManager();
            mgr.AddArgs(args);

            Assert.AreEqual(mgr.GetValue("name"), "Eriri");
            Assert.AreEqual(mgr.GetValue("full-name"), "Sawamura Spencer Eriri");
            Assert.IsTrue(mgr.GetBool("enable"));
            Assert.AreEqual(mgr.GetValue("CV"), "Oonishi Saori");
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
