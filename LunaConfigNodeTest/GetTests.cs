using System.Linq;
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
            var value = configNode.GetValues("field1").FirstOrDefault()?.Value;

            Assert.AreEqual("field1Val", value);
        }

        [TestMethod]
        public void TestGetNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var nodes = configNode.GetNodes("Node1");

            Assert.AreEqual(1, nodes.Count);
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
