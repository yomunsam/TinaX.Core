using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TinaX.IO;

namespace TinaX.Tests.Core
{
    public class XPathTests
    {
        // A Test behaves as an ordinary method
        [Test]
        [TestCase("Assets/Img/1.png",true,".png")]
        [TestCase("Assets/Lua/hello.lua.txt",true,".lua.txt")]
        [TestCase("Assets/Lua/hello.lua.txt",false,".txt")]
        public void GetExtension_Single(string path, bool Multiple, string Expect)
        {
            if (path.IsNullOrEmpty() || Expect.IsNullOrEmpty()) return;
            var result = XPath.GetExtension(path, Multiple);
            TestContext.Out.WriteLine($"Path: {path}, multiple:{Multiple.ToString()} , result: {result}");
            if(result == Expect)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }

        //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        //// `yield return null;` to skip a frame.
        //[UnityTest]
        //public IEnumerator XPathTestsWithEnumeratorPasses()
        //{
        //    // Use the Assert class to test conditions.
        //    // Use yield to skip a frame.
        //    yield return null;
        //}
    }
}
