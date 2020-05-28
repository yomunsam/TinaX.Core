using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TinaX.Tests.Core
{
    public class JsonUtilityTest
    {

        [Test]
        public void TestSerializeDict()
        {
            var myData = new Dictionary<string, string>();
            myData.Add("Id", "0");
            myData.Add("Name", "Alice");

            //Serialize 
            var json1 = JsonUtility.ToJson(new Serialization<string,string>(myData));
            TestContext.WriteLine("Json 1: \n" + json1);

            var json_obj1 = JsonUtility.FromJson<Serialization<string, string>>(json1)
                .ToDictionary();
            Assert.AreEqual(myData, json_obj1);

            //By Ext Method
            var json2 = myData.ToJson();
            TestContext.WriteLine("Json 2: \n" + json2);
            var json_obj2 = JsonHelper.FromJsonToDictionary<string, string>(json2);
            Assert.AreEqual(json_obj1, json_obj2);
        }

        [Test]
        public void TestSerializeList()
        {
            var myData = new List<AModel>
            {
                new AModel("Alice", Guid.NewGuid()),
                new AModel("Bob", Guid.NewGuid())
            };

            //serialize
            var json1 = JsonUtility.ToJson(new Serialization<AModel>(myData));
            TestContext.WriteLine("Json 1: \n" + json1);

            var json_obj1 = JsonUtility.FromJson<Serialization<AModel>>(json1)
                .ToList();
            //Assert.AreEqual(myData, json_obj1);

            //Ext Method
            var json2 = myData.ToJson();
            TestContext.WriteLine("Json 2:\n" + json2);

            var json_obj2 = JsonHelper.FromJsonToList<AModel>(json2);
            //Assert.AreEqual(json_obj1, json_obj2);

        }

#pragma warning disable CA2235 // Mark all non-serializable fields

        [System.Serializable]
#pragma warning disable CA1034 // Nested types should not be visible
        public class AModel
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public string Name;
            public string Guid;

            public override string ToString()
            {
                return $"Name:{this.Name}\nGuid:{this.Guid}";
            }

            public AModel(string name, Guid guid)
            {
                Name = name;
                this.Guid = guid.ToString();
            }
        }
#pragma warning restore CA2235 // Mark all non-serializable fields

    }
}
