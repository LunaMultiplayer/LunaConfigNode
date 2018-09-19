using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class UpdateTests
    {
        [TestMethod]
        public void TestUpdateValue()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.UpdateValue("field2", "field2NewVal");

            Assert.AreEqual("field2NewVal", configNode.GetValue("field2"));
        }

        [TestMethod]
        public void TestUpdateNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var newNode = new ConfigNode { Name = "Node2" };
            configNode.UpdateNode(newNode);

            Assert.AreEqual(newNode, configNode.GetNodes("Node2")[0]);
        }

        [TestMethod]
        public void TestUpdateSeveralNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var newNode = new ConfigNode { Name = "RepeatedNode3" };
            configNode.UpdateNode(newNode);

            Assert.AreEqual(newNode, configNode.GetNodes("RepeatedNode3")[0]);
            Assert.AreEqual(newNode, configNode.GetNodes("RepeatedNode3")[1]);
        }
    }
}
