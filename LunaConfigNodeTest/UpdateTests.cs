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
    }
}
