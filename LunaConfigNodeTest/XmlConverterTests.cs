using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using LunaConfigNode;
using LunaConfigNodeTest.Extension;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class XmlConverterTests
    {
        private static readonly string XmlExamplePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Examples", "Others");

        [TestMethod]
        public void TestSeveralVesselsToXml()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(XmlExamplePath)))
            {
                SwitchToXmlAndBack(file);
            }
        }

        private static void SwitchToXmlAndBack(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var configNode = File.ReadAllText(filePath).ToUnixString();
            var xml = XmlConverter.ConvertToXml(configNode);
            var backToConfigNode = XmlConverter.ConvertToConfigNode(xml).ToUnixString();

            Assert.IsTrue(configNode.Equals(backToConfigNode), $"Error serializing config node. File: {Path.GetFileName(filePath)}");
        }
    }
}
