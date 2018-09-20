using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class GetterTests
    {
        [TestMethod]
        public void TestGetValue()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var value = configNode.GetValue("field1");

            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestGetNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var node = configNode.GetNode("Node1");

            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void TestGetRepeatedNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var nodes = configNode.GetNodes("RepeatedNode3");

            Assert.AreEqual(2, nodes.Count);
        }
    }
}
