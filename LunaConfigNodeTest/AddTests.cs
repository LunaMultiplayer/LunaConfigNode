﻿using LunaConfigNode.CfgNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class AddTests
    {
        [TestMethod]
        public void TestAddValueRoot()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.CreateValue(new CfgNodeValue<string, string>("name", "testValue"));

            Assert.IsNotNull(configNode.GetValues("name"));
        }

        [TestMethod]
        public void TestAddValueSubNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0].Value;
            subNode.CreateValue(new CfgNodeValue<string, string>("name", "testValue"));

            Assert.IsNotNull(subNode.GetValues("name"));
        }
        
        [TestMethod]
        public void TestAddNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.AddNode(new ConfigNode("NewNode", configNode));

            var subNode = configNode.GetNodes("NewNode");
            Assert.AreEqual(1, subNode.Count);
        }
    }
}
