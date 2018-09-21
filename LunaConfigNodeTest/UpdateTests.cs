using LunaConfigNode.CfgNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class UpdateTests
    {
        [TestMethod]
        public void TestUpdateValueDirectly()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.GetValue("field2").Value = "field2NewVal";

            Assert.AreEqual("field2NewVal", configNode.GetValue("field2").Value);
        }

        [TestMethod]
        public void TestUpdateValue()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.UpdateValue("field2", "field2NewVal");

            Assert.AreEqual("field2NewVal", configNode.GetValues("field2")[0].Value);
        }

        [TestMethod]
        public void TestUpdateNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var newNode = new ConfigNode("Node2", null);
            configNode.ReplaceNode(newNode);

            Assert.AreEqual(newNode, configNode.GetNodes("Node2")[0].Value);
        }

        [TestMethod]
        public void TestUpdateSeveralNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var newNode = new ConfigNode("RepeatedNode3", configNode);
            configNode.ReplaceNode(newNode);

            Assert.AreEqual(newNode, configNode.GetNodes("RepeatedNode3")[0].Value);
            Assert.AreEqual(newNode, configNode.GetNodes("RepeatedNode3")[1].Value);
        }
    }
}
