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

            //This is done to normalize the new line on linux (appveyor)
            contentAsString = contentAsString.Replace("\r\n", "\n");
            contentAsString = contentAsString.Replace("\n", "\r\n");

            Assert.AreEqual(Resources.Vessel, contentAsString);
        }
    }
}
