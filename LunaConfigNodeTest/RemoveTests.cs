using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class RemoveTests
    {
        [TestMethod]
        public void TestRemoveValueRoot()
        {
            var configNode = new ConfigNode(Resources.Simple);
            configNode.RemoveValue("field1");

            Assert.IsNull(configNode.GetValue("field1"));
        }

        [TestMethod]
        public void TestRemoveValueSubNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0];
            subNode.RemoveValue("field11");

            Assert.IsNull(subNode.GetValue("field11"));
        }
        
        [TestMethod]
        public void TestRemoveNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0];
            configNode.RemoveNode(subNode);

            Assert.AreEqual(0, configNode.GetNodes("Node1").Count);
        }
    }
}
