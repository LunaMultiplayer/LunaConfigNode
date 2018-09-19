using System;
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

            var areEqual = string.Equals(Resources.Vessel, contentAsString, StringComparison.Ordinal);
            Assert.IsTrue(areEqual);
        }
    }
}
