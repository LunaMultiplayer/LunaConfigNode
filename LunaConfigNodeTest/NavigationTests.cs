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
            var result = content.Navigate(out var part, "PART", "name", "solarPanels2");

            Assert.IsTrue(result);
            Assert.IsNotNull(part);
        }
    }
}
