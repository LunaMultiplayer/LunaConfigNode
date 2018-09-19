using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void TestWriteConfigNode()
        {
            var asUnixStr = Resources.Vessel.Replace(Environment.NewLine, "\n");

            var content = new ConfigNode(asUnixStr);
            var contentAsString = content.ToString().Replace(Environment.NewLine, "\n");
            
            var areEqual = string.Equals(asUnixStr, contentAsString, StringComparison.Ordinal);
            Assert.IsTrue(areEqual);
        }
    }
}
