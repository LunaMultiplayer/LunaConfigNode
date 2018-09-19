using LunaConfigNode;
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
            configNode.AddValue("name", "testValue");

            Assert.IsNotNull(configNode.GetValue("name"));
        }

        [TestMethod]
        public void TestAddValueSubNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0];
            subNode.AddValue("name", "testValue");

            Assert.IsNotNull(subNode.GetValue("name"));
        }
        
        [TestMethod]
        public void TestAddNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.AddNode("NewNode");

            var subNode = configNode.GetNodes("NewNode");
            Assert.AreEqual(1, subNode.Count);
        }
    }
}
