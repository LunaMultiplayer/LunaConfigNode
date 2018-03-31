using LunaConfigNode;
using LunaConfigNodeTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunaConfigNodeTest
{
    [TestClass]
    public class ReaderTests
    {
        [TestMethod]
        public void TestReadFromContent()
        {
            var content = Reader.ReadFromContent(Resources.Vessel);
            Assert.IsNotNull(content);

            var contentAsString = content.ToString();
            Assert.AreEqual(Resources.Vessel, contentAsString);
        }
    }
}
