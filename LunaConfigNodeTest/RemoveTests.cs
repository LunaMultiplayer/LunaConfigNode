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

            Assert.AreEqual(0, configNode.GetValues("field1").Count);
        }

        [TestMethod]
        public void TestRemoveValueSubNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0];
            subNode.RemoveValue("field11");

            Assert.AreEqual(0, subNode.GetValues("field11").Count);
        }
        
        [TestMethod]
        public void TestRemoveNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("Node1")[0];
            configNode.RemoveNode(subNode);

            Assert.AreEqual(0, configNode.GetNodes("Node1").Count);
        }

        [TestMethod]
        public void TestRemoveOneOfSeveralNodes()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("RepeatedNode3")[0];
            configNode.RemoveNode(subNode);

            Assert.AreEqual(1, configNode.GetNodes("RepeatedNode3").Count);
        }

        [TestMethod]
        public void TestRemoveAllOfSeveralNodes()
        {
            var configNode = new ConfigNode(Resources.Simple);
            var subNode = configNode.GetNodes("RepeatedNode4")[0];
            configNode.RemoveNode(subNode);

            Assert.AreEqual(0, configNode.GetNodes("RepeatedNode4").Count);
        }
    }
}
