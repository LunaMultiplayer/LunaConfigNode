using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class NavigationTests
    {
        [TestMethod]
        public void TestNavigate()
        {
            var content = Reader.ReadFromContent(Resources.Vessel);
            var result = content.TryGetValue(out var value, "PART", "name", "solarPanels2");

            Assert.IsTrue(result);
            Assert.IsNotNull(value);

            var partNode = value.Parent;
            result = partNode.TryGetValue(out value, "MODULE", "name", "ModuleDeployableSolarPanel");

            Assert.IsTrue(result);
            Assert.IsNotNull(value);
        }
    }
}
