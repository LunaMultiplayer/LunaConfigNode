using LunaConfigNode.CfgNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class ReplaceTests
    {
        [TestMethod]
        public void TestReplaceSimpleNode()
        {
            var configNode = new ConfigNode(Resources.Simple);
            Assert.AreEqual(1, configNode.GetNodes("Node2").Count);
            var oldNode = configNode.GetNode("Node2").Value;

            var newNode = new ConfigNode("NewNode2", null);
            configNode.ReplaceNode(oldNode, newNode);

            Assert.AreEqual(0, configNode.GetNodes("Node2").Count);
            Assert.AreEqual(1, configNode.GetNodes("NewNode2").Count);
        }

        [TestMethod]
        public void TestReplaceNodeRepeatedInName()
        {
            var configNode = new ConfigNode(Resources.Simple);
            Assert.AreEqual(2, configNode.GetNodes("RepeatedNode3").Count);
            var oldNode = configNode.GetNodes("RepeatedNode3")[0];

            var newNode = new ConfigNode("NonRepeatedNode3", null);
            configNode.ReplaceNode(oldNode.Value, newNode);

            Assert.AreEqual(1, configNode.GetNodes("RepeatedNode3").Count);
        }

        [TestMethod]
        public void TestReplaceNodeThatIsCloned()
        {
            var configNode = new ConfigNode(Resources.Simple);
            Assert.AreEqual(2, configNode.GetNodes("RepeatedNode4").Count);
            var oldNode = configNode.GetNodes("RepeatedNode4")[0];

            var newNode = new ConfigNode("NonRepeatedNode4", null);
            configNode.ReplaceNode(oldNode.Value, newNode);

            Assert.AreEqual(0, configNode.GetNodes("RepeatedNode4").Count);
        }
    }
}
