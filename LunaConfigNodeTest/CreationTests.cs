using LunaConfigNode.CfgNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class CreationTests
    {
        [TestMethod]
        public void TestCreateConfigNode()
        {
            var content = new ConfigNode(Resources.Vessel);
            Assert.IsNotNull(content);

            Assert.AreNotEqual(0, content.GetAllValues().Count);
        }
    }
}
