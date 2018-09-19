using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void TestWriteConfigNode()
        {
            var content = new ConfigNode(Resources.Vessel);
            var contentAsString = content.ToString();

            Assert.AreEqual(Resources.Vessel, contentAsString);
        }
    }
}
